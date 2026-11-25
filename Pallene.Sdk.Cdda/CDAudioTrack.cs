using System;
using System.Collections.Generic;
using System.Text;

namespace Pallene.Sdk.Cdda
{
    public sealed class CDAudioTrack
    {
        public string? Isrc
        {
            get;
            set
            {
                if(value != null) CddaValidators.ThrowIfNotValidIsrc(value);
                field = value;
            }
        }

        public CDTimecode Length
        {
            get => audioIndexes.Aggregate<CDStreamWrapper, CDTimecode>(new(), (acc, next) => acc += next.Length);
        }

        private List<CDStreamWrapper> audioIndexes = new(2);

        public CDStreamWrapper this[int index] 
        {
            get => audioIndexes[index];
        }

        public CDAudioTrack(Stream audioStream, string? isrc = null)
        {
            audioIndexes[0] = CDStreamWrapper.Empty; // Empty pregap
            audioIndexes[1] = new CDStreamWrapper(audioStream);
            Isrc = isrc;
        }

        public void SetPregap(Stream stream) => audioIndexes[0] = new CDStreamWrapper(stream);

        public void RemovePregap(Stream stream) => audioIndexes[0] = CDStreamWrapper.Empty;


        public void AddSpecialIndex(Stream stream)
        {
            CddaValidators.ThrowIfNotValidIndexNumber(audioIndexes.Count);
            audioIndexes.Add(new CDStreamWrapper(stream));
        }

        public void InsertSpecialIndex(int index, Stream stream)
        {
            CddaValidators.ThrowIfNotValidIndexNumber(index);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(index, 1);
            CddaValidators.ThrowIfNotValidIndexNumber(audioIndexes.Count);
            audioIndexes.Insert(index, new CDStreamWrapper(stream));
        }

        /// <summary>
        /// Removes the special index at the position specified.
        /// </summary>
        /// <param name="index">The index to remove</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RemoveSpecialIndex(int index)
        {
            CddaValidators.ThrowIfNotValidIndexNumber(index);
            ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(index, 1);
            audioIndexes.RemoveAt(index);
        } 

        internal CDTimecode[] GetOffsets()
        {
            CDTimecode[] res = new CDTimecode[audioIndexes.Count];
            res[0] = new();
            for(int i = 1; i < res.Length; i++) 
                res[i] = res[i - 1] + audioIndexes[i].Length;
            return res;
        }

        internal CueSheetEntry[] GetCueSheetEntries(
            CDTimecode baseOffset, byte trackNumber, ControlField controlField = ControlField.None,
            SubchannelDataForm subchannelDataForm = SubchannelDataForm.NoData,
            SerialCopyManagement copyManagement = SerialCopyManagement.None
            )
        {
            CueSheetEntry[] isrcEntries = Isrc != null ? CueSheetEntry.GenerateIsrcEntry(controlField, trackNumber, Isrc) : Array.Empty<CueSheetEntry>();
            CueSheetEntry[] indexEntries = new CueSheetEntry[audioIndexes.Count];
            CDTimecode[] offsets = GetOffsets();
            for(byte i = 0; i < indexEntries.Length; i++)
            {
                indexEntries[i] = CueSheetEntry.GenerateTrackEntry(
                    controlField, trackNumber, i,
                    audioIndexes[i].HasContent ? MainDataForm.AudioData : MainDataForm.AudioNoData,
                    subchannelDataForm,
                    copyManagement,
                    baseOffset + offsets[i]
                    );
            }
            return [.. isrcEntries, .. indexEntries];
        }
    }
}

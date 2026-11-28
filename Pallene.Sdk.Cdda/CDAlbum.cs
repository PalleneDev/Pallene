using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Pallene.Sdk.Cdda.Tests")]

namespace Pallene.Sdk.Cdda
{
    public sealed class CDAlbum
    {
        public string? Mcn
        {
            get;
            set
            {
                if (value != null) CddaValidators.ThrowIfNotValidMcn(value);
                field = value;
            }
        }

        private List<CDAudioTrack> trackList = new();

        public CDTimecode Length
        {
            get => trackList.Aggregate<CDAudioTrack, CDTimecode>(new(), (acc, next) => acc += next.Length);
        }

        public CDAudioTrack this[int trackNumber]
        {
            get => trackList[trackNumber - 1];
        }

        public void Add(CDAudioTrack track) => trackList.Add(track);

        public void Insert(int trackNumber, CDAudioTrack track)
        {
            CddaValidators.ThrowIfNotValidTrackNumber(trackNumber);
            trackList.Insert(trackNumber - 1, track);
        }

        public void Remove(int trackNumber) => trackList.RemoveAt(trackNumber - 1);

        internal void CheckValidity()
        {
            if (this[1][0].Length < new CDTimecode(0,2,0))
                throw new InvalidOperationException("Index 0 of Track 1 must be at least 2 seconds long.");
            if (trackList.Any(track => (track.Length - track[0].Length) < new CDTimecode(0, 4, 0)))
                throw new InvalidOperationException("Every track must be at least 4 seconds long (not counting its Index 0).");
        }

        internal CueSheetEntry[] GetCueSheetEntries()
        {
            CheckValidity();
            List<CueSheetEntry> result = new();
            if (Mcn != null)
                result.AddRange(CueSheetEntry.GenerateMcnEntry(Mcn));
            result.Add(CueSheetEntry.GenerateLeadInEntry());
            CDTimecode offset = new();
            for(int i = 0; i < trackList.Count; i++)
            {
                CDAudioTrack track = trackList[i];
                result.AddRange(
                    track.GetCueSheetEntries(
                        offset,
                        (byte)(i + 1)
                        )
                    );
                offset += track.Length;
            }
            result.Add(CueSheetEntry.GenerateLeadOutEntry(offset));
            return result.ToArray();
        }
    }
}

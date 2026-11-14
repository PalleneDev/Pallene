using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

[assembly: InternalsVisibleTo("Pallene.Sdk.Cdda.Tests")]

namespace Pallene.Sdk.Cdda
{

    [StructLayout(LayoutKind.Sequential)]
    internal struct CueSheetEntry
    {
        private byte controlAddressField;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
        private byte[] dataBytes;


        private CueSheetEntry(ControlField controlField, AddressField addressField, byte[] dataBytes)
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(dataBytes.Length, 7);
            controlAddressField = (byte)((byte)controlField + (byte)addressField);
            this.dataBytes = dataBytes;
        }

        public static CueSheetEntry[] GenerateMcnEntry(string catalogNumber)
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(catalogNumber.Length, 13); // The length of a UPC/EAN code.
            catalogNumber += '\0'; // Normalize to 14 bytes -- 7 per entry
            byte[] catalogNumberBuffer = Encoding.ASCII.GetBytes(catalogNumber);

            return [
                new(ControlField.None, AddressField.CatalogCode, catalogNumberBuffer[0..6]),
            new(ControlField.None, AddressField.CatalogCode, catalogNumberBuffer[7..13])
                ];
        }

        public static CueSheetEntry[] GenerateIsrcEntry(ControlField controlField, byte trackNumber, string recordingCode)
        {
            ArgumentOutOfRangeException.ThrowIfNotEqual(recordingCode.Length, 12); // The length of an ISRC

            byte[] recordingCodeBuffer = Encoding.ASCII.GetBytes(recordingCode);

            return [
                    new(controlField, AddressField.IsrcCode, [trackNumber, ..recordingCodeBuffer[0..5]]),
                new(controlField, AddressField.IsrcCode, [trackNumber, ..recordingCodeBuffer[6..11]])
                ];
        }

        public static CueSheetEntry GenerateTrackEntry(
            ControlField controlField, byte trackNumber, byte index, MainDataForm mainDataForm,
            SubchannelDataForm subchannelDataForm, SerialCopyManagement serialCopyManagement, CDTimecode timecode
            )
            =>
            new CueSheetEntry(
                controlField,
                AddressField.TrackInfo,
                [trackNumber, index, (byte)((byte)subchannelDataForm + (byte)mainDataForm), (byte)serialCopyManagement, .. (byte[])timecode]
                );


        public static implicit operator byte[](CueSheetEntry input) => [input.controlAddressField, .. input.dataBytes];

        public static byte[] Flatten(IEnumerable<CueSheetEntry> entries) => entries.SelectMany(entry => (byte[])entry).ToArray();
    }
}
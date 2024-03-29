﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace FizzlePuzzle.Utility
{
    /* From http://answers.unity3d.com/questions/737002/wav-byte-to-audioclip.html */
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    internal class WAV
    {
        // convert two bytes to one float in the range -1 to 1
        private static float BytesToFloat(byte firstByte, byte secondByte)
        {
            // convert two bytes to one short (little endian)
            short s = (short) ((secondByte << 8) | firstByte);
            // convert to range from -1 to (just below) 1
            return s / 32768.0F;
        }

        private static int BytesToInt(IReadOnlyList<byte> bytes, int offset = 0)
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                value |= bytes[offset + i] << (i * 8);
            }

            return value;
        }

        // properties
        internal float[] LeftChannel { get; }
        internal float[] RightChannel { get; }
        internal int ChannelCount { get; }
        internal int SampleCount { get; }
        internal int Frequency { get; }

        internal WAV(IReadOnlyList<byte> wav)
        {
            // Determine if mono or stereo
            ChannelCount = wav[22]; // Forget byte 23 as 99.999% of WAVs are 1 or 2 channels

            // Get the frequency
            Frequency = BytesToInt(wav, 24);

            // Get past all the other sub chunks to get to the data subchunk:
            int pos = 12; // First Subchunk ID from 12 to 16

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }

            pos += 8;

            // Pos is now positioned to start of actual sound data.
            SampleCount = (wav.Count - pos) / 2; // 2 bytes per sample (16 bit sound mono)
            if (ChannelCount == 2)
            {
                SampleCount /= 2; // 4 bytes per sample (16 bit stereo)
            }

            // Allocate memory (right will be null if only mono sound)
            LeftChannel = new float[SampleCount];
            RightChannel = ChannelCount == 2 ? new float[SampleCount] : null;

            // Write to double array/s:
            int i = 0;
            int maxInput = wav.Count - (RightChannel == null ? 1 : 3);
            // while (pos < wav.Length)
            while ((i < SampleCount) && (pos < maxInput))
            {
                LeftChannel[i] = BytesToFloat(wav[pos], wav[pos + 1]);
                pos += 2;
                if (ChannelCount == 2)
                {
                    Debug.Assert(RightChannel != null, nameof(RightChannel) + " != null");
                    RightChannel[i] = BytesToFloat(wav[pos], wav[pos + 1]);
                    pos += 2;
                }

                i++;
            }
        }
    }
}

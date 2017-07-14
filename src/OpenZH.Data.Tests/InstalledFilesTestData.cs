﻿using System;
using System.IO;
using System.Linq;
using OpenZH.Data.Big;
using Xunit.Abstractions;

namespace OpenZH.Data.Tests
{
    internal static class InstalledFilesTestData
    {
        public static void ReadFiles(string fileExtension, ITestOutputHelper output, Action<string, Func<Stream>> processFileCallback)
        {
            var rootDirectories = new[]
            {
                @"C:\Program Files (x86)\Origin Games\Command and Conquer Generals Zero Hour\Command and Conquer Generals",
                @"C:\Program Files (x86)\Origin Games\Command and Conquer Generals Zero Hour\Command and Conquer Generals Zero Hour"
            };

            foreach (var directory in rootDirectories.Where(x => Directory.Exists(x)))
            {
                foreach (var bigFile in Directory.GetFiles(directory, "*.big", SearchOption.AllDirectories))
                {
                    output.WriteLine($"Reading BIG archive {Path.GetFileName(bigFile)}.");

                    using (var bigStream = File.OpenRead(bigFile))
                    using (var archive = new BigArchive(bigStream))
                    {
                        foreach (var entry in archive.Entries.Where(x => Path.GetExtension(x.FullName).ToLower() == fileExtension))
                        {
                            output.WriteLine($"Reading file {entry.FullName}.");

                            processFileCallback(entry.FullName, entry.Open);
                        }
                    }
                }
            }
        }
    }
}

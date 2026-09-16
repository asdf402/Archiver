namespace Archiver
{
    public class Append : ICommand
    {
        public Result Execute(ParsedArguments parsedArguments)
        {
            long endOfFilePosition;

            ArchiveReader archiveReader = new ArchiveReader();
            MetaInformation metaInformation;

            try
            {
                FileStream destination = new FileStream(parsedArguments.Destination, FileMode.Open);
                using (BinaryReader binaryReader = new BinaryReader(destination))
                {
                    metaInformation = archiveReader.ReadAllMetaInformation(binaryReader);
                    endOfFilePosition = binaryReader.BaseStream.Length;
                }
            }
            catch (IOException)
            {
                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenDestination, parsedArguments.Destination);
            }

            return this.AppendFiles(parsedArguments, metaInformation, endOfFilePosition);
        }

        public Result AppendFiles(ParsedArguments parsedArguments, MetaInformation metaInformation, long startingPosition)
        {
            long fileSizeCompressedPosition;

            FileInformation fileInformation;
            ArchiveWriter archiveWriter = new ArchiveWriter();
            EnumerationOptions enumerationOptions = new EnumerationOptions
            {
                IgnoreInaccessible = true,
                ReturnSpecialDirectories = false,
                MaxRecursionDepth = 100,
            };

            try
            {
                int counter = 0;

                // gets all directories including root directory
                IEnumerable<string> directories =
                    Directory.GetDirectories(parsedArguments.Source, "*", enumerationOptions)
                    .Prepend(parsedArguments.Source);

                FileStream destination = new FileStream(parsedArguments.Destination, FileMode.Open);
                using (BinaryWriter binaryWriter = new BinaryWriter(destination))
                {
                    // changes the stream position to the given starting position
                    binaryWriter.BaseStream.Position = startingPosition;

                    // gets the next directory from the directories enumerable
                    string? nextDirectory = this.GetNextDirectory(directories, counter);

                    // checks if there are still directories left
                    while (nextDirectory != null)
                    {
                        // gets every file from the directory
                        IEnumerable<string> files = Directory.EnumerateFiles(nextDirectory);

                        foreach (var file in files)
                        {
                            // saves the position to later write the file size compressed there
                            fileSizeCompressedPosition = binaryWriter.BaseStream.Position;

                            // creates the new file object with all the necessary info
                            fileInformation = new FileInformation(
                                Path.GetRelativePath(parsedArguments.Source, file),
                                new FileInfo(file).Length,
                                0);

                            // writes the info to the file
                            archiveWriter.WriteFileInformation(binaryWriter, fileInformation);

                            try
                            {
                                // writes the file itself in the archive
                                FileStream source = new FileStream(file, FileMode.Open);
                                using (BinaryReader binaryReader = new BinaryReader(source))
                                {
                                    fileInformation.AddFileSizeCompressed(
                                        metaInformation.Compress.Execute(binaryReader, binaryWriter));
                                }
                            }
                            catch (IOException)
                            {
                                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenSource, file);
                            }

                            // updates the metainformationobject for every file
                            metaInformation.AddFileSizeUncompressed(fileInformation.FileSizeUncompressed);
                            metaInformation.IncreaseFileAmount();

                            // updates the file size compressed at the before saved position
                            archiveWriter.WriteAddedFileSizeCompressed(binaryWriter, fileSizeCompressedPosition, fileInformation);
                        }

                        // gets the next directory from the directories enumerable
                        nextDirectory = this.GetNextDirectory(directories, ++counter);
                    }

                    // writes the updated meta information to the file
                    archiveWriter.WriteChangedMetaInformation(binaryWriter, metaInformation.FilesSizeUncompressed, metaInformation.FileAmount);
                }

                return new Result(false, ConsoleErrorOutput.WriteNoErrorOccurred, "AppendFiles");
            }
            catch (IOException)
            {
                return new Result(true, ConsoleErrorOutput.WriteCouldNotOpenDestination, parsedArguments.Destination);
            }
        }

        private string? GetNextDirectory(IEnumerable<string> directories, int counter)
        {
            return directories.ElementAtOrDefault(counter);
        }
    }
}

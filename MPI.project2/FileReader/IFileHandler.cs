﻿using MPI.project2.Data;

namespace MPI.project2.FileReader;

public interface IFileHandler
{
    General ReadDataFromFile(string fileName);
}
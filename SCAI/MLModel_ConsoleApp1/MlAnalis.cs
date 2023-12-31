﻿
// This file was auto-generated by ML.NET Model Builder. 

using MLModel_ConsoleApp1;
using System.IO;

public class MlAnalis
{
    private static void Main(string[] args)
    {
        string path = "";
        Analise(path);
    }

    public static Dictionary<string, float> Analise(string imagePath) 
    {
        var imageBytes = File.ReadAllBytes(imagePath);
        MLModel.ModelInput sampleData = new MLModel.ModelInput()
        {
            ImageSource = imageBytes,
        };
        var sortedScoresWithLabel = MLModel.PredictAllLabels(sampleData);
        var result = new Dictionary<string, float>();
        foreach (var label in sortedScoresWithLabel) 
        {
            result.Add(label.Key, label.Value);
        }
        return result;
    }
}
// Scaffold-DbContext "Host=localhost;Port=5432;Database=SCAI_DB;Username=postgres;Password=123321" Npgsql.EntityFrameworkCore.PostgreSQL
// Npgsql.EntityFrameworkCore.PostgreSQL
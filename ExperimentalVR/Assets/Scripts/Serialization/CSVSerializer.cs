using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Serialization;
using UnityEngine;

public class CSVSerializer
{
    private static char _csvSeparator = ',';
    private static List<Type> _nonPrimitive;

    public static void Generate(IEnumerable data, ref List<string> linesInCSV, char separator = ',')
    {
        InitializeNonPrimitives();

        InitLines(ref linesInCSV);
        GenerateHeaders(data, true, ref linesInCSV, separator);
        GenerateBody(data, ref linesInCSV, separator);
    }

    private static void InitializeNonPrimitives()
    {
        _nonPrimitive = new List<Type>();
//        _nonPrimitive.Add(typeof(GazeValidationData));
//        _nonPrimitive.Add(typeof(FocusInfo));
//        _nonPrimitive.Add(typeof(WrapperValidationData));
//        _nonPrimitive.Add(typeof(WrapperGazeValidationData));
//        _nonPrimitive.Add(typeof(SpecificGazeValidationData));
//        _nonPrimitive.Add(typeof(EyeTrackingData));
//        _nonPrimitive.Add(typeof(IKWEye.Data.SingleEyeData));
//        _nonPrimitive.Add(typeof(IKWEye.Data.CombinedEyeData));
//        _nonPrimitive.Add(typeof(FocusData));
//        _nonPrimitive.Add(typeof(HeadData));
//        _nonPrimitive.Add(typeof(EventData));
    }

    private static void InitLines(ref List<string> linesInCsv)
    {
        linesInCsv.Add("");
    }


    private static void GenerateHeaders(IEnumerable data, bool isFirstRun, ref List<string> linesInCSV,
        char separator, bool isInnerCall = false)
    {
        foreach (object dataPoint in data)
        {
            if (isFirstRun)
            {
                //Beginn of adding Information
                IEnumerable<Attribute> customAttributes =
                    dataPoint.GetType().GetCustomAttributes(typeof(SerializableName));
                string baseName = "";
                if (customAttributes.Count() != 0)
                    baseName = ((SerializableName) customAttributes.First()).BaseName + "_";
                else
                    baseName = dataPoint.GetType().Name;
                //Get the variables in the class
                FieldInfo[] fields = dataPoint.GetType()
                    .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (FieldInfo fieldInfo in fields)
                {
                    if (!fieldInfo.FieldType.IsGenericType)
                    {
                        linesInCSV[linesInCSV.Count - 1] += ExpandWithUnityValuesHeader(fieldInfo.FieldType,
                            baseName + fieldInfo.Name, separator);
                        if (fieldInfo.Equals(fields.Last()))
                        {
                            linesInCSV[linesInCSV.Count - 1]
                                .Remove(linesInCSV[linesInCSV.Count - 1].LastIndexOf(separator));
                        }
                    }
                    else
                    {
                        GenerateHeaders(fieldInfo.GetValue(dataPoint) as IEnumerable, true, ref linesInCSV,
                            separator, true);
                    }
                }

                isFirstRun = false;
                //End of adding information
            }
        }

        linesInCSV.Add("");
    }

    private static object GetFirstItem(IEnumerable data)
    {
        List<object> dataPointList = new List<object>();
        foreach (object dataPoint in data)
        {
            dataPointList.Add(dataPoint);
            break;
        }

        return dataPointList[0];
    }

    private static void GenerateBody(IEnumerable data, ref List<string> linesInCSV, char separator = ',')
    {
        foreach (object dataPoint in data)
        {
            FieldInfo[] fields = dataPoint.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (FieldInfo fieldInfo in fields)
            {
                if (!fieldInfo.FieldType.IsGenericType)
                {
                    linesInCSV[linesInCSV.Count - 1] += ExpandWithUnityValuesData(fieldInfo.FieldType,
                        fieldInfo.GetValue(dataPoint),
                        separator);
                }
                else
                {
                    GenerateBody(fieldInfo.GetValue(dataPoint) as IEnumerable, ref linesInCSV);
                }
            }

            linesInCSV.Add("");
        }
    }


    private static string ExpandWithUnityValuesHeader(Type fieldType, string fieldName,
        char separator = ',')
    {
        string expandedHeader = "";
        if (fieldType == typeof(Vector3))
        {
            expandedHeader = fieldName + ".x" + separator +
                             fieldName + ".y" + separator +
                             fieldName + ".z" + separator;
        }
        else if (fieldType == typeof(Quaternion))
        {
            expandedHeader = fieldName + ".x" + separator +
                             fieldName + ".y" + separator +
                             fieldName + ".z" + separator +
                             fieldName + ".w" + separator;
        }
        else if (fieldType == typeof(Ray))
        {
            expandedHeader = fieldName + ".origin.x" + separator +
                             fieldName + ".origin.y" + separator +
                             fieldName + ".origin.z" + separator +
                             fieldName + ".direction.x" + separator +
                             fieldName + ".direction.y" + separator +
                             fieldName + ".direction.z" + separator;
        }
        else if (_nonPrimitive.Contains(fieldType))
        {
            expandedHeader = ExpandWithNonPrimitveTypeHeader(fieldType, fieldName);
        }
        else
        {
            expandedHeader = fieldName + separator;
        }

        return expandedHeader;
    }

    private static string ExpandWithNonPrimitveTypeHeader(Type fieldType, string fieldName)
    {
        IEnumerable<Attribute> customAttributes = fieldType.GetCustomAttributes(typeof(SerializableName));
        string baseName = "";
        FieldInfo[] fields =
            fieldType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            string partialCSVHeader = "";

        if (customAttributes.Count() != 0)
            baseName = ((SerializableName) customAttributes.First()).BaseName + "_";
        else
            baseName = fieldName;

        foreach (FieldInfo fieldInfo in fields)
        {
            partialCSVHeader += ExpandWithUnityValuesHeader(fieldInfo.FieldType,
                baseName + fieldInfo.Name, _csvSeparator);
        }

        return partialCSVHeader;
    }

    private static string ExpandWithUnityValuesData(Type fieldType, object fieldValue,
        char separator = ',')
    {
        string expandedData = "";
        if (fieldValue != null)
        {
            if (fieldType == typeof(Vector3))
            {
                expandedData = ((Vector3) fieldValue).x.ToString() + separator +
                               ((Vector3) fieldValue).y.ToString() + separator +
                               ((Vector3) fieldValue).z.ToString() + separator;
            }
            else if (fieldType == typeof(Quaternion))
            {
                expandedData = ((Quaternion) fieldValue).x.ToString() + separator +
                               ((Quaternion) fieldValue).y.ToString() + separator +
                               ((Quaternion) fieldValue).z.ToString() + separator +
                               ((Quaternion) fieldValue).w.ToString() + separator;
            }
            else if (fieldType == typeof(Ray))
            {
                expandedData = ((Ray) fieldValue).origin.x.ToString() + separator +
                               ((Ray) fieldValue).origin.y.ToString() + separator +
                               ((Ray) fieldValue).origin.z.ToString() + separator +
                               ((Ray) fieldValue).direction.x.ToString() + separator +
                               ((Ray) fieldValue).direction.y.ToString() + separator +
                               ((Ray) fieldValue).direction.z.ToString() + separator;
            }
            else if (_nonPrimitive.Contains(fieldType))
            {
                expandedData = ExpandWithNonPrimitiveData(fieldType, fieldValue);
            }
            else
            {
                expandedData = fieldValue.ToString() + separator;
            }
        }
        else
        {
            expandedData = "NULL" + separator;
        }

        return expandedData;
    }


    private static string ExpandWithNonPrimitiveData(Type fieldType, object fieldValue)
    {
        string partialCSVData = "";
        FieldInfo[] fields =
            fieldType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        foreach (FieldInfo fieldInfo in fields)
        {
            partialCSVData +=
                ExpandWithUnityValuesData(fieldInfo.FieldType, fieldInfo.GetValue(fieldValue));
        }

        return partialCSVData;
    }

    public static bool SaveCSV(List<string> csvLines, string fileAddress, string fileName,
        string prefix = "", char fileNameSeperetor = '-', string extension = ".csv", string customCSVHeaders = "")
    {
        try
        {
            prefix = prefix.Equals("") ? DataIOManager.instance.GetDefaultPrefix() : prefix;
            //Prefix fist, because we want to order the files by the prefixes, as an example: the VP-Number
            AddHeaderAndCheckDirectoryExistince(fileAddress, customCSVHeaders, csvLines);
            using (FileStream fileStream =
                new FileStream(fileAddress + prefix + fileNameSeperetor + fileName + extension,
                    FileMode.OpenOrCreate))
            {
                using (StreamWriter csvWriter =
                    new StreamWriter(fileStream))
                {
                    foreach (string csvLine in csvLines)
                    {
                        csvWriter.WriteLine(csvLine);
                    }

                    csvWriter.Close();
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
            throw;
        }

        return true;
    }

    private static void AddHeaderAndCheckDirectoryExistince(string fileAddress, string customCSVHeaders,
        List<string> csvLines)
    {
        if (customCSVHeaders != "")
        {
            csvLines.RemoveAt(0);
            csvLines.Insert(0, customCSVHeaders);
        }

        if (!Directory.Exists(fileAddress))
            Directory.CreateDirectory(fileAddress);
    }

    public static void GenerateAndSaveCSV(IEnumerable data, string fileAddress, string fileName)
    {
//        if (data.GetType() == typeof(List<EyeTrackingValidationData>))
//        {
//            GenerateAndSaveEyeTrackingValidationData(data as List<EyeTrackingValidationData>, fileAddress,
//                fileName);
//        }
//        else
//        {
        InnerGerateAndSaveCSV(data, fileAddress, fileName);
//        }
    }

//    private static void GenerateAndSaveEyeTrackingValidationData(
//        List<EyeTrackingValidationData> eyeTrackingValidationData, string fileAddress, string fileName)
//    {
//        List<WrapperValidationData> wrapperPointValidationData = new List<WrapperValidationData>();
//        List<WrapperGazeValidationData> wrapperGazeValidationData = new List<WrapperGazeValidationData>();
//
//        foreach (EyeTrackingValidationData validationData in eyeTrackingValidationData)
//        {
//            wrapperPointValidationData.Add(new WrapperValidationData(validationData.GetValidationPoint(),
//                validationData.GetLastPointScale(), validationData.GetMeasuringTime(),
//                validationData.GetValidationTrial()));
//            foreach (GazeValidationData gazeValidationData in validationData.GetGazeValidation())
//            {
//                wrapperGazeValidationData.Add(new WrapperGazeValidationData(validationData.GetValidationTrial(),
//                    validationData.GetValidationPoint(), gazeValidationData));
//            }
//        }
//
//        //TODO mach aus den parametern klassen variablen
//        InnerGerateAndSaveCSV(wrapperPointValidationData, fileAddress, fileName + "PointValidation");
//        InnerGerateAndSaveCSV(wrapperGazeValidationData, fileAddress, fileName + "GazeValidation");
//    }

//    internal class WrapperGazeValidationData
//    {
//        int trailNumber;
//        string pointName;
////        SpecificGazeValidationData _leftEyeGazeValidationData;
////        SpecificGazeValidationData _rightEyeGazeValidationData;
////        SpecificGazeValidationData _combinedEyeGazeValidationData;
//
//        public WrapperGazeValidationData(int trailNumber, string pointName, GazeValidationData gazeValidationData)
//        {
//            this.trailNumber = trailNumber;
//            this.pointName = pointName;
//            _leftEyeGazeValidationData = gazeValidationData.LeftEyeGazeValidationData;
//            _rightEyeGazeValidationData = gazeValidationData.RightEyeGazeValidationData;
//            _combinedEyeGazeValidationData = gazeValidationData.CombinedEyeGazeValidationData;
//        }
//    }

    private static void InnerGerateAndSaveCSV(IEnumerable data, string fileAddress, string fileName)
    {
        List<string> linesInCSV = new List<string>();
        Generate(data, ref linesInCSV);
        SaveCSV(linesInCSV, fileAddress, fileName);
    }

//    internal class WrapperValidationData
//    {
//        String _pointName;
//        Vector3 _lastPointScale;
//        float _measuringTime;
//        int _validationTrial;
//
//        public WrapperValidationData(string pointName, Vector3 lastPointScale, float measuringTime,
//            int validationTrial)
//        {
//            _pointName = pointName;
//            _lastPointScale = lastPointScale;
//            _measuringTime = measuringTime;
//            _validationTrial = validationTrial;
//        }
//    }
}
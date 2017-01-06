////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Autodesk, Inc. All rights reserved 
// Written by Philippe Leefsma 2012 - ADN/Developer Technical Services
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

using Inventor;
using Autodesk.ADN.InvUtility.InventorUtils;

namespace MaterialProfiler
{
    class MaterialUtils
    {
        public class MaterialInfos
        {
            public double Density
            {
                get;
                internal set;
            }

            public double Volume
            {
                get;
                set;
            }

            public double DbMass
            {
                get;
                set;
            }

            public double Mass
            {
                get;
                set;
            }
            
            public double MassPercentage
            {
                get;
                set;
            }
            
            public MaterialInfos(Material material)
            {
                this.Density = material.Density;
                this.Volume = 0.0;
                this.Mass = 0.0;
                this.MassPercentage = 0.0;
            }
        }

        public class MaterialGlobalInfos
        {
            public Material Material
            {
                get;
                internal set;
            }

            public MaterialInfos GlobalInfos
            {
                get;
                set;
            }

            public Dictionary<ComponentOccurrence, MaterialInfos> BreakDownInfos
            {
                get;
                internal set;
            }

            public MaterialGlobalInfos(
                AssemblyDocument document, 
                Material material, 
                List<ComponentOccurrence> materialOccurrences)
            {
                MassProperties asmMassProperties = document.ComponentDefinition.MassProperties;

                Material = material;

                GlobalInfos = new MaterialInfos(material);

                BreakDownInfos = new Dictionary<ComponentOccurrence, MaterialInfos>();

                string docVolumeUnits = GetDocVolumeUnits(document as Document, false);
                string dbVolumeUnits = GetDbVolumeUnits(document as Document, false);

                foreach (ComponentOccurrence occurrence in materialOccurrences)
                {
                    MaterialInfos materialInfos = new MaterialInfos(material);

                    //Compute percentage first, so no need to convert in doc units first
                    materialInfos.MassPercentage = 
                        occurrence.MassProperties.Mass * 100.0 / asmMassProperties.Mass;

                    materialInfos.DbMass = occurrence.MassProperties.Mass;

                    materialInfos.Volume = document.UnitsOfMeasure.ConvertUnits(
                        occurrence.MassProperties.Volume,
                        dbVolumeUnits,
                        docVolumeUnits);

                    materialInfos.Mass = document.UnitsOfMeasure.ConvertUnits(
                        occurrence.MassProperties.Mass,
                        UnitsTypeEnum.kDatabaseMassUnits,
                        UnitsTypeEnum.kDefaultDisplayMassUnits);

                    BreakDownInfos.Add(occurrence, materialInfos);

                    //Add to global, database value
                    GlobalInfos.DbMass += materialInfos.DbMass;
                    GlobalInfos.Volume += occurrence.MassProperties.Volume;
                    GlobalInfos.Mass += occurrence.MassProperties.Mass;
                }

                //Compute percentage first, so no need to convert in doc units first
                GlobalInfos.MassPercentage = GlobalInfos.Mass * 100.0 / asmMassProperties.Mass;

                //then convert to doc units
                GlobalInfos.Volume = document.UnitsOfMeasure.ConvertUnits(
                    GlobalInfos.Volume,
                    dbVolumeUnits,
                    docVolumeUnits);

                GlobalInfos.Mass = document.UnitsOfMeasure.ConvertUnits(
                    GlobalInfos.Mass,
                    UnitsTypeEnum.kDatabaseMassUnits,
                    UnitsTypeEnum.kDefaultDisplayMassUnits);
            }
        }

        public static string GetDocMassUnits(Document document)
        {
            UnitsOfMeasure unitsOfMeasure = document.UnitsOfMeasure;
            string stringFromValue = unitsOfMeasure.GetStringFromValue(0.0, unitsOfMeasure.MassUnits);
            return stringFromValue.Split(new char[]{' '})[1];
        }

        public static string GetDocVolumeUnits(Document document, bool appendSymbol = true)
        {
            UnitsOfMeasure unitsOfMeasure = document.UnitsOfMeasure;

            string stringFromValue = unitsOfMeasure.GetStringFromValue(0.0, unitsOfMeasure.LengthUnits);

            string volUnit = stringFromValue.Split(new char[]	{' '})[1];

            return (appendSymbol ? volUnit + "^3" : volUnit + " " + volUnit + " " + volUnit);
        }

        public static string GetDbVolumeUnits(Document document, bool appendSymbol = true)
        {
            UnitsOfMeasure unitsOfMeasure = document.UnitsOfMeasure;

            string stringFromValue = unitsOfMeasure.GetStringFromValue(0.0, UnitsTypeEnum.kDatabaseLengthUnits);

            string volUnit = stringFromValue.Split(new char[] { ' ' })[1];

            return (appendSymbol ? volUnit + "^3" : volUnit + " " + volUnit + " " + volUnit);
        }

        public static string GetDocDensityUnits(Document document, bool appendSymbol = true)
        {
            return GetDocMassUnits(document) + "/" + GetDocVolumeUnits(document, appendSymbol);
        }

        public static double GetDocDensity(Document document, double ApiDensity)
        {
            UnitsOfMeasure unitsOfMeasure = document.UnitsOfMeasure;

            double num = unitsOfMeasure.ConvertUnits(1.0, "g", unitsOfMeasure.MassUnits);
            double num2 = unitsOfMeasure.ConvertUnits(1.0, "cm^3", GetDocVolumeUnits(document));

            return ApiDensity * num / num2;
        }

        public static string FormatVisible(double value)
        {
            string valueStr = value.ToString("F3");

            if (double.Parse(valueStr) != 0.0)
                return valueStr;

            return string.Format("{0,16:0.000e+00}", value);
        }

        private static bool MatchingMaterial(Material mat1, Material mat2)
        {
            return (mat1.Name == mat2.Name);
        }

        public static Dictionary<Material, MaterialGlobalInfos> 
            GetMaterialInfos(AssemblyDocument document)
        {
            Dictionary<Material, MaterialGlobalInfos> result;

            try
            {
                Dictionary<Material, List<ComponentOccurrence>> dictionary = 
                    new Dictionary<Material, List<ComponentOccurrence>>();

                foreach (ComponentOccurrence componentOccurrence in 
                    document.ComponentDefinition.Occurrences.get_AllLeafOccurrences(Missing.Value))
                {
                    Material material = AdnInventorUtilities.GetProperty(
                        componentOccurrence.Definition, 
                        "Material") 
                            as Material;

                    if (material != null)
                    {
                        bool flag = false;

                        foreach (Material current in dictionary.Keys)
                        {
                            if (MatchingMaterial(material, current))
                            {
                                dictionary[current].Add(componentOccurrence);
                                flag = true;
                                break;
                            }
                        }

                        if (!flag)
                        {
                            dictionary.Add(material, new List<ComponentOccurrence>{componentOccurrence});
                        }
                    }
                }

                Dictionary<Material, MaterialGlobalInfos> dictionary2 = new Dictionary<Material, MaterialGlobalInfos>();
                
                foreach (Material current2 in dictionary.Keys)
                {
                    MaterialGlobalInfos value = new MaterialGlobalInfos(
                        document, 
                        current2, 
                        dictionary[current2]);

                    dictionary2.Add(current2, value);
                }

                result = dictionary2;
            }
            catch
            {
                result = null;
            }

            return result;
        }
    }
}

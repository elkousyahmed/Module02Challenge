using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module02Challenge
{
    [Transaction(TransactionMode.Manual)]
    public class Challenge02 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;

            IList<Element> pickElementsFromRvtFile = uidoc.Selection.PickElementsByRectangle("select elementsss using rectangle window!");

            TaskDialog.Show("selectoin quantity", "you selected " + pickElementsFromRvtFile.Count.ToString() + " elements from revit");


            //uidoc.Selection.PickPoint();



            /*----------------------------------------------------------****

            
            // filter for curve elements only (model lline or detail line)

            List<CurveElement> AllCurves = new List<CurveElement>();
            foreach (Element elem in pickElementsFromRvtFile)
            {
                if (elem is CurveElement)
                {
                    AllCurves.Add(elem as CurveElement);
                }
            }

            ----------------------------------------------------------*/



            // filter for the model lines only (model curve)

            List<CurveElement> modelCurveOnly = new List<CurveElement>();
            foreach (Element elem in pickElementsFromRvtFile)
            {
                if (elem is CurveElement)
                {
                    CurveElement curveElem = (CurveElement)elem; // ---→ or  = elem as CurveElem 

                    if (curveElem.CurveElementType == CurveElementType.ModelCurve)
                    {
                        modelCurveOnly.Add(curveElem);
                    }

                }

            }





            using (Transaction t = new Transaction(doc))
            {

                t.Start("Create Elements");
                Level level1 = Level.Create(doc, 100);


                foreach (CurveElement line in modelCurveOnly)
                {
                    Curve c1 = line.GeometryCurve;



                    ////A-GLAZ
                    if (line.LineStyle.Name == "A-GLAZ")
                    {

                        Wall w = Wall.Create(doc, c1, level1.Id, false);

                    }

                    //A-WALL
                    if (line.LineStyle.Name == "A-WALL")
                    {
                        //FilteredElementCollector wallTypeFilter = new FilteredElementCollector(doc).OfClass(typeof(WallType));

                        WallType w = new FilteredElementCollector(doc)
                            .OfCategory(BuiltInCategory.OST_Walls)
                            .WhereElementIsElementType().FirstElement() as WallType;


                        Wall.Create(doc, c1, w.Id, level1.Id, 15, 0, false, false);

                    }

                    //M-DUCT


                    if (line.LineStyle.Name == "M-DUCT")
                    {

                        Wall w = Wall.Create(doc, c1, level1.Id, false);
                    }



                    //P-PIPE
                    if (line.LineStyle.Name == "P-PIPE")
                    {

                        Wall w = Wall.Create(doc, c1, level1.Id, false);

                    }

                }




                t.Commit();

            }


            return Result.Succeeded;
        }
    }
}

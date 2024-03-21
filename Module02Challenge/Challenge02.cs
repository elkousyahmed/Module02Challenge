using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
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

            /*
             * 
             * 
            
            // filter for curve elements only (model lline or detail line)

            List<CurveElement> AllCurves = new List<CurveElement>();
            foreach (Element elem in pickElementsFromRvtFile)
            {
                if (elem is CurveElement)
                {
                    AllCurves.Add(elem as CurveElement);
                }
            }

            *
            *
            *
            */



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


            #region try01
            /*
            foreach (CurveElement curveElem in modelCurveOnly)
            {
                switch (curveElem.LineStyle)
                {
                    case "A-GLAZ":

                    default:
                        break;
                }
            } 
            */
            #endregion

            using (Transaction t = new Transaction(doc))
            {

                t.Start("Create Elements");

                Level level1 = Level.Create(doc, 100);
                
                foreach (CurveElement curveElem in modelCurveOnly)
                {
                    //A-GLAZ
                    if (curveElem.LineStyle.Name == "A-GLAZ")
                    {
                        Wall w = Wall.Create(doc, curveElem, level1.Id, false);


                    }

                    //A-WALL
                    else if (curveElem.LineStyle.Name == "A-WALL")
                    {



                    }

                    //M-DUCT
                    else if (curveElem.LineStyle.Name == "M-DUCT")
                    {



                    }

                    //P-PIPE
                    else if (curveElem.LineStyle.Name == "P-PIPE")
                    {




                    }

                }

                t.Commit();

            }


            return Result.Succeeded;
        }
    }
}

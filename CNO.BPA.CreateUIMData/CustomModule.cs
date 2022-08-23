using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CNO.BPA.CreateUIMData
{
    using Emc.InputAccel.CaptureClient;
    
    public class CustomModule : CustomCodeModule
    {

        public CustomModule()
        {
        }

        public override void ExecuteTask(IClientTask task, IBatchContext batchContext)
        {
            /*
            Dim abc As IUimDataContext
            abc = task.BatchNode.NodeData.NewUimData("DocType")
            MessageBox.Show(abc.DocumentName)
            */
            try {
                if (task.BatchNode.RootLevel != 3) {
                    throw new Exception("The module must be triggered at Level 3");
                }
                else {

                    IBatchNode desktopStep = batchContext.GetStepNode(task.BatchNode, "ManualIndex");
                    Emc.InputAccel.UimScript.IUimDataContext e_uimData = desktopStep.NodeData.NewUimData("BPAStandardDocType");
                    string e_uimDataString = desktopStep.NodeData.UimDataToString(e_uimData);
                    desktopStep.NodeData.ValueSet.WriteString("UimData", e_uimDataString);

                    foreach (IBatchNode doc in desktopStep.GetDescendantNodes(1))
                    {
                        Emc.InputAccel.UimScript.IUimDataContext uimData = doc.NodeData.NewUimData("BPAStandardDocType");
                        string uimDataString = doc.NodeData.UimDataToString(uimData);

                        doc.NodeData.ValueSet.WriteString("UimData", uimDataString);

                    }
                    task.CompleteTask();
                }

            } catch(Exception e) {
                task.FailTask(FailTaskReasonCode.GenericUnrecoverableError, e );
            }
        }

        public override void StartModule(ICodeModuleStartInfo startInfo)
        {
        }

    }
}

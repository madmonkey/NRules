﻿using System.IO;
using Microsoft.VisualStudio.DebuggerVisualizers;
using NRules.Diagnostics;

namespace NRules.Debugger.Visualizer
{
    public class SessionObjectSource : VisualizerObjectSource
    {
        public override void GetData(object target, Stream outgoingData)
        {
            var session = (ISession) target;
            var schema = session.GetSchema();
            var dgmlWriter = new DgmlWriter(schema);
            var contents = dgmlWriter.GetContents();
            base.GetData(contents, outgoingData);
        }
    }
}
﻿/* ****************************************************************************
 *
 * Copyright (c) Microsoft Corporation. 
 *
 * This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
 * copy of the license can be found in the License.html file at the root of this distribution. If 
 * you cannot locate the Apache License, Version 2.0, please send an email to 
 * vspython@microsoft.com. By using this source code in any fashion, you are agreeing to be bound 
 * by the terms of the Apache License, Version 2.0.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * ***************************************************************************/

using System;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Input;
using EnvDTE;

namespace TestUtilities.UI.Nodejs {
    public class NodejsVisualStudioApp : VisualStudioApp {
        private NodejsPerfExplorer _perfTreeView;
        private NodejsPerfToolBar _perfToolBar;
        public NodejsVisualStudioApp(DTE dte)
            : base(dte) {
        }

        /// <summary>
        /// Opens and activates the solution explorer window.
        /// </summary>
        public void OpenNodejsPerformance() {
            Dte.ExecuteCommand("View.Node.jsPerformanceExplorer");
        }

        /// <summary>
        /// Opens and activates the Node.js Performance explorer window.
        /// </summary>
        public void LaunchNodejsProfiling() {
            ThreadPool.QueueUserWorkItem(x => Dte.ExecuteCommand("Analyze.LaunchNode.jsProfiling"));
        }

        /// <summary>
        /// Provides access to the Python profiling tree view.
        /// </summary>
        public NodejsPerfExplorer NodejsPerformanceExplorerTreeView {
            get {
                if (_perfTreeView == null) {
                    var element = Element.FindFirst(TreeScope.Descendants,
                        new AndCondition(
                            new PropertyCondition(
                                AutomationElement.ClassNameProperty,
                                "SysTreeView32"
                            ),
                            new PropertyCondition(
                                AutomationElement.NameProperty,
                                "Node.js Performance"
                            )
                        )
                    );
                    _perfTreeView = new NodejsPerfExplorer(element);
                }
                return _perfTreeView;
            }
        }

        /// <summary>
        /// Provides access to the Python profiling tool bar
        /// </summary>
        public NodejsPerfToolBar NodejsPerformanceExplorerToolBar {
            get {
                if (_perfToolBar == null) {
                    var element = Element.FindFirst(TreeScope.Descendants,
                        new AndCondition(
                            new PropertyCondition(
                                AutomationElement.ClassNameProperty,
                                "ToolBar"
                            ),
                            new PropertyCondition(
                                AutomationElement.NameProperty,
                                "Node.js Performance"
                            )
                        )
                    );
                    _perfToolBar = new NodejsPerfToolBar(element);
                }
                return _perfToolBar;
            }
        }

        public Document WaitForDocument(string docName) {
            for (int i = 0; i < 100; i++) {
                try {
                    return Dte.Documents.Item(docName);
                } catch {
                    System.Threading.Thread.Sleep(100);
                }
            }
            throw new InvalidOperationException("Document not opened: " + docName);
        }
    }
}
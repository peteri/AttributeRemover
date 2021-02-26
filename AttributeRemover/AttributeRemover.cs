// <copyright file="AttributeRemover.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AttributeRemover
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Microsoft.Build.Framework;

    /// <summary>
    /// Removes InternalVisibleTo attributes from a net assembly.
    /// </summary>
    public class AttributeRemover : Microsoft.Build.Utilities.Task, IDisposable
    {
        /// <summary>
        /// Gets or Sets list of assemblies that will have attributes removed.
        /// </summary>
        [Required]
        public virtual ITaskItem[] InputAssemblies { get; set; } = new ITaskItem[0];

        /// <summary>
        /// Gets or Sets list of attributes remove.
        /// </summary>
        [Required]
        public virtual ITaskItem[] Attributes { get; set; } = new ITaskItem[0];

        /// <summary>
        /// Don't have anything to kill.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Actually does the work.
        /// </summary>
        /// <returns>true, if successful.</returns>
        public override bool Execute()
        {
            for (int i = 0; i < InputAssemblies.Length; i++)
            {
                try
                {
                    string assemblyPath = InputAssemblies[i].ItemSpec;
                    if (string.IsNullOrEmpty(assemblyPath))
                    {
                        throw new Exception($"Invalid assembly path on item index {i}");
                    }

                    if (!File.Exists(assemblyPath) && !File.Exists(BuildPath(assemblyPath)))
                    {
                        throw new Exception($"Unable to resolve assembly '{assemblyPath}'");
                    }

                    Log.LogMessage(MessageImportance.Normal, "Removing attributes from assembly '{0}'", assemblyPath);
                }
                catch (Exception ex)
                {
                    Log.LogError($"Error trying to remove attributes {ex}");
                }
            }

            // Log.HasLoggedErrors is true if the task logged any errors -- even if they were logged
            // from a task's constructor or property setter. As long as this task is written to always log an error
            // when it fails, we can reliably return HasLoggedErrors.
            return !Log.HasLoggedErrors;
        }

        /// <summary>
        /// Returns path respective to current working directory.
        /// </summary>
        /// <param name="path">Relative path to current working directory.</param>
        /// <returns>Returns path.</returns>
        private string BuildPath(string path)
        {
            var workDir = Directory.GetCurrentDirectory();
            return string.IsNullOrEmpty(path) ? null : Path.Combine(workDir, path);
        }
    }
}

// <copyright file="AttributeRemover.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

namespace AttributeRemover
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.Build.Framework;
    using Mono.Cecil;

    /// <summary>
    /// Removes InternalVisibleTo attributes from a net assembly.
    /// </summary>
    public class AttributeRemover : Microsoft.Build.Utilities.Task
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
        public virtual string Attributes { get; set; }

        /// <summary>
        /// Actually does the work.
        /// </summary>
        /// <returns>true, if successful.</returns>
        public override bool Execute()
        {
            string[] attributes = Attributes.Split(new[] { ',' });
            Log.LogMessage(MessageImportance.High, "Attributes to remove '{0}'", string.Join(";", attributes));

            for (int i = 0; i < InputAssemblies.Length; i++)
            {
                try
                {
                    string assemblyPath = InputAssemblies[i].ItemSpec;
                    if (string.IsNullOrEmpty(assemblyPath))
                    {
                        throw new Exception($"Invalid assembly path on item index {i}");
                    }

                    if (!File.Exists(assemblyPath))
                    {
                        throw new Exception($"Unable to resolve assembly '{assemblyPath}'");
                    }

                    Log.LogMessage(MessageImportance.High, "Removing attributes from assembly '{0}'", assemblyPath);

                    RemoveAttributes(assemblyPath, attributes);
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

        private void RemoveAttributes(string assemblyPath, string[] attributesToRemove)
        {
            List<(string name, bool used)> attr = attributesToRemove.Select(s => (s, false)).ToList();

            var assembly = AssemblyDefinition.ReadAssembly(assemblyPath);
            if (assembly.HasCustomAttributes)
            {
                foreach (var customAttribute in assembly.CustomAttributes)
                {
                    if (customAttribute.AttributeType.FullName == "System.Runtime.CompilerServices.InternalsVisibleToAttribute")
                    {
                        assembly.CustomAttributes.Remove(customAttribute);
                    }
                }
            }

            string[] unusedAttr = attr.Where(at => at.used == false).Select(at => at.name).ToArray();
            if (unusedAttr.Any())
            {
                Log.LogWarning("Did not remove attributes {0}", string.Join(",", unusedAttr));
            }

            assembly.Write();
        }
    }
}

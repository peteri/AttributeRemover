// <copyright file="InternalsVisibleTo.cs" company="Peter Ibbotson">
// Copyright (c) Peter Ibbotson. All rights reserved.
// </copyright>

using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("AttributeRemover.Tests")]

namespace AttributeRemover.TestAssembly
{
    /// <summary>
    /// Test class used to check InternalsVisibleToAttributes can be removed.
    /// </summary>
    internal class InternalsVisibleTo
    {
        /// <summary>
        /// Test method to see if loading the assembly before and after build works.
        /// </summary>
        public void CanYouSeeMe()
        {
            throw new NotImplementedException();
        }
    }
}

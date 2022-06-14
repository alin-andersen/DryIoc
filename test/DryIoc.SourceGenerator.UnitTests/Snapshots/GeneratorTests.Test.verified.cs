﻿//HintName: CompileTimeContainer.generated.cs
namespace DryIoc.SourceGenerator.UnitTests
{
    using System;
    using System.Collections.Generic;
    using DryIoc;
    using DryIoc.CompileTime;
    using DryIoc.ImTools;
    
    public partial class CompileTimeContainer : ICompileTimeContainer
    {
        public void ResolveGenerated(ref object service, Type serviceType)
        {
        }

        public void ResolveGenerated(ref object service, Type serviceType, object serviceKey, Type requiredServiceType, Request preRequestParent, object[] args)
        {
        }

        public IEnumerable<ResolveManyResult> ResolveManyGenerated(Type serviceType)
        {
            yield break;
        }
    }
}
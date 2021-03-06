#region license
// Copyright (c) 2003, 2004, 2005 Rodrigo B. de Oliveira (rbo@acm.org)
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without modification,
// are permitted provided that the following conditions are met:
// 
//     * Redistributions of source code must retain the above copyright notice,
//     this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice,
//     this list of conditions and the following disclaimer in the documentation
//     and/or other materials provided with the distribution.
//     * Neither the name of Rodrigo B. de Oliveira nor the names of its
//     contributors may be used to endorse or promote products derived from this
//     software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
// FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
// DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
// CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
// OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
// THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

using Boo.Lang.Compiler.TypeSystem.Internal;
using Boo.Lang.Compiler.TypeSystem.Services;
using Boo.Lang.Environments;

namespace Boo.Lang.Compiler.Steps
{
	using Boo.Lang.Compiler.Ast;

    /// <summary>
	/// Pre-binds methods and constructors before resolving type references, 
	/// to enable correct resolution of generic type references.
	/// </summary>
	public class BindMethods : AbstractVisitorCompilerStep, ITypeMemberReifier
	{
		private InternalTypeSystemProvider _internalTypeSystemProvider;

		public override void Initialize(CompilerContext context)
		{
			base.Initialize(context);
			_internalTypeSystemProvider = My<InternalTypeSystemProvider>.Instance;

		}
		override public void OnMethod(Method node)
		{
			EnsureEntityFor(node);
			Visit(node.ExplicitInfo);
		}

		protected void EnsureEntityFor(TypeMember node)
		{
			_internalTypeSystemProvider.EntityFor(node);
		}

		public override void OnConstructor(Constructor node)
		{
			EnsureEntityFor(node);
		}
		
		override public void OnExplicitMemberInfo(ExplicitMemberInfo node)
		{
			Visit(node.InterfaceType);
		}
		
		override public void OnClassDefinition(ClassDefinition node)
		{
			Visit(node.Members);
		}
		
		override public void OnModule(Module node)
		{
			Visit(node.Members);
		}
		
		override public void Run()
		{			
			Visit(CompileUnit.Modules);
		}

        public virtual TypeMember Reify(TypeMember member)
        {
        	member.Accept(this);
        	return member;
        }
	}
}

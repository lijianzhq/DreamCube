﻿using sdmap.Compiler;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace sdmap.test
{
    public class DefDepsTest
    {
        [Fact]
        public void Smoke()
        {
            var rt = new SdmapCompiler();
            rt.AddSourceCode("sql v1{#def<id, 'test'>#deps<id>}");
            string result = rt.Emit("v1", null);
            Assert.Equal("test", result);
        }

        [Fact]
        public void WillKeepsOrder()
        {
            var rt = new SdmapCompiler();
            rt.AddSourceCode("sql v1{#def<C, 'C'>#def<A, 'A'>#def<B, sql{B}>#deps<A,B,C>}");
            string result = rt.Emit("v1", null);
            Assert.Equal("CAB", result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WillLoadOnCondition(bool load)
        {
            var rt = new SdmapCompiler();
            rt.AddSourceCode("sql v1{#def<A, 'A'>#if(A){#deps<A>B}}");
            string result = rt.Emit("v1", new { A = load });

            Assert.Equal(load ? "AB" : "", result);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void WillLoadOnEqual(bool load)
        {
            var rt = new SdmapCompiler();
            rt.AddSourceCode("sql v1{#def<A, 'A'>#isEqual<A, true, sql {#deps<A>B}>}");
            string result = rt.Emit("v1", new { A = load });

            Assert.Equal(load ? "AB" : "", result);
        }

        [Fact]
        public void DefDepEmitNothing()
        {
            var rt = new SdmapCompiler();
            rt.AddSourceCode("sql v1{#def<A, 'A'>#def<B, sql {#deps<A>B}}");
            string result = rt.Emit("v1", null);
            Assert.Equal("", result);
        }

        [Fact]
        public void DefDepEmitAll()
        {
            var rt = new SdmapCompiler();
            rt.AddSourceCode("sql v1{#def<A, 'A'>#def<B, sql{#deps<A>B}>#deps<B>}");
            string result = rt.Emit("v1", null);
            Assert.Equal("AB", result);
        }

        [Fact]
        public void ReferencedDefDepInMacro()
        {
            var rt = new SdmapCompiler();
            rt.AddSourceCode("sql v1{#def<A, 'A'>#def<B, sql{#deps<A>B}>#isNotNull<B, sql {#deps<B>C}}}");
            string result = rt.Emit("v1", new { B = "" });
            Assert.Equal("ABC", result);
        }

        [Fact]
        public void IncludedDefAlsoWorks()
        {
            var rt = new SdmapCompiler();
            rt.AddSourceCode("sql v1{#include<v2>3#deps<B>} sql v2{1#def<B, '2'>}");
            string result = rt.Emit("v1", null);
            Assert.Equal("123", result);
        }
    }
}

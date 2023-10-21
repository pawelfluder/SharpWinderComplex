﻿using SharpContainerProg.Public;
using SharpTinderGoogleDocsTests.Repetition;

namespace SharpTinderComplexTests.Repetition
{
    internal static class MyBorder
    {
        private static IContainer container01 = new SharpSetupProg24Private.Repetition.Registration().Start();

        private static IContainer container = new Registration().Start();
        public static IContainer Container => container;
    }
}

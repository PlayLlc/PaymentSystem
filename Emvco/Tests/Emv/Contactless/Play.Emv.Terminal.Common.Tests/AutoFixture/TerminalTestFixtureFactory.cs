﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Ber.TestData.AutoFixture;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel2.Databases;
using Play.Emv.Terminal.Common.Tests.TerminalActionAnalysisServiceTests;

namespace Play.Emv.Terminal.Common.Tests.AutoFaq
{
    public class TerminalTestFixtureFactory
    {
        #region Static Metadata

        private static readonly List<ISpecimenBuilder> _SpecimenBuilders;

        #endregion

        #region Constructor

        static TerminalTestFixtureFactory()
        {
            SpecimenForeman foreman = new();
            foreman.Build(foreman.RegisteredApplicationProviderIndicator);

            _SpecimenBuilders = foreman.Complete();
        }

        #endregion

        #region Instance Members

        /// <exception cref="NotSupportedException"></exception>
        public static IFixture Create()
        {
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());

            foreach (ISpecimenBuilder item in _SpecimenBuilders)
                fixture.Customizations.Add(item);

            Setup(fixture);

            return fixture;
        }

        private static void Setup(IFixture fixture)
        {
            fixture.Register<TerminalActionCodeDefault>(() => new TerminalActionCodeDefault((ulong) TerminalActionAnalysisServiceFactory
                                                                                                .GetRandomTerminalActionCodeDefault()));
            fixture.Register<TerminalActionCodeDenial>(() => new TerminalActionCodeDenial((ulong) TerminalActionAnalysisServiceFactory
                                                                                              .GetRandomTerminalActionCodeDenial()));
            fixture.Register<TerminalActionCodeOnline>(() => new TerminalActionCodeOnline((ulong) TerminalActionAnalysisServiceFactory
                                                                                              .GetRandomTerminalActionCodeOnline()));
        }

        #endregion
    }
}
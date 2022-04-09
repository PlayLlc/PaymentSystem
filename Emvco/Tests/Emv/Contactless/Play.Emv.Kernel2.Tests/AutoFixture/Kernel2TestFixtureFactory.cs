using System;
using System.Collections.Generic;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;

using Play.Ber.DataObjects;
using Play.Emv.Ber.DataElements;
using Play.Emv.Kernel.Contracts;
using Play.Emv.Kernel.Databases;
using Play.Emv.Kernel2.Tests.TerminalActionAnalysisServiceTests;
using Play.Testing.Infrastructure.AutoFixture.SpecimenBuilders;

namespace Play.Emv.Kernel2.Tests.AutoFixture
{
    public class Kernel2TestFixtureFactory
    {
        #region Static Metadata

        private static readonly List<ISpecimenBuilder> _SpecimenBuilders;

        #endregion

        #region Constructor

        static Kernel2TestFixtureFactory()
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

            fixture.Register(() => new KernelDatabase(fixture.Create<CertificateAuthorityDataset[]>(),
                                                      new Kernel2PersistentValues(Array.Empty<PrimitiveValue>()),
                                                      new Kernel2KnownObjects()));

            fixture.Freeze<KernelDatabase>();
        }

        #endregion
    }
}
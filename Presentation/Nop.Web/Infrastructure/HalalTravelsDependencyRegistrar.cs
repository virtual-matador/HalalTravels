using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Domain.Hotels;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Services.Hotels;
using Nop.Services.Zones;
using Nop.Web.Areas.Admin.Factories;

namespace Nop.Web.Infrastructure
{
    /// <summary>
    /// Dependency registrar
    /// </summary>
    public class HalalTravelsDependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder">Container builder</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="config">Config</param>
        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            //admin factories
            builder.RegisterType<HotelModelFactory>().As<IHotelModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<HotelTypeModelFactory>().As<IHotelTypeModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ChainModelFactory>().As<IChainModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<HotelCategoryModelFactory>().As<IHotelCategoryModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<HotelCategoryModelFactory>().As<IHotelCategoryModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<PricingModelModelFactory>().As<IPricingModelModelFactory>().InstancePerLifetimeScope();
            builder.RegisterType<ContractTypeModelFactory>().As<IContractTypeModelFactory>().InstancePerLifetimeScope();

            //services
            builder.RegisterType<HotelService>().As<IHotelService>().InstancePerLifetimeScope();
            builder.RegisterType<HotelTypeService>().As<IHotelTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<ChainService>().As<IChainService>().InstancePerLifetimeScope();
            builder.RegisterType<CountryService>().As<ICountryService>().InstancePerLifetimeScope();
            builder.RegisterType<ProvinceService>().As<IProvinceService>().InstancePerLifetimeScope();
            builder.RegisterType<CityService>().As<ICityService>().InstancePerLifetimeScope();
            builder.RegisterType<CountyService>().As<ICountyService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
            builder.RegisterType<HotelTagService>().As<IHotelTagService>().InstancePerLifetimeScope();
            builder.RegisterType<PricingModelService>().As<IPricingModelService>().InstancePerLifetimeScope();
            builder.RegisterType<ContractTypeService>().As<IContractTypeService>().InstancePerLifetimeScope();
            builder.RegisterType<DepartmentService>().As<IDepartmentService>().InstancePerLifetimeScope();
        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 2;
    }
}
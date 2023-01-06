using AerolineasU4_WPF.Models;
using AerolineasU4_WPF.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AerolineasU4_WPF.ViewModels
{
    internal class AerolineasViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Vuelum> Vuelos { get; set; } = new ObservableCollection<Vuelum>();
        public List<Vuelum> Vuelums { get; set; } = new List<Vuelum>();

        public TimeSpan minu { get; set; } = new TimeSpan(0, 5 ,0);

        readonly AerolineasWPFService serviceVuelo = new();


        public AerolineasViewModel()
        {
            CargarVuelos();

            DispatcherTimer act = new DispatcherTimer();
            act.Interval = TimeSpan.FromSeconds(5);
            act.Tick += Timer_Tick;
            act.Start();

            DispatcherTimer Borrar = new DispatcherTimer();
            Borrar.Interval = TimeSpan.FromMinutes(1);
            Borrar.Tick += Timer_Borrar;
            Borrar.Start();
        }

        private void Timer_Borrar(object? sender, EventArgs e)
        {
            VuelosCanAt();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            CargarVuelos();

        }

        async void CargarVuelos()
        {
            Vuelos.Clear();
            var datos = await serviceVuelo.Get();

            foreach (var vm in datos)
            {
                TimeSpan min = (vm.Hora).Subtract(DateTime.Now);

                if (vm.Hora < DateTime.Now && vm.Estado != "CANCELADO")
                {
                    vm.Estado = "DESPEGO";
                    await serviceVuelo.Update(vm);
                }
                else if (min < minu && vm.Estado != "CANCELADO")
                {
                    vm.Estado = "ABORDANDO";
                    await serviceVuelo.Update(vm);
                }
            }

            datos.ForEach(x => Vuelos.Add(x));

        }

        private async void VuelosCanAt()
        {
            var vuelos = await serviceVuelo.GetDespegoCancelado();
            if (vuelos!=null)
            {
                foreach (var item in vuelos)
                {
                    await serviceVuelo.Delete(item);
                }
            }
            
            CargarVuelos();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Actualizar(string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}

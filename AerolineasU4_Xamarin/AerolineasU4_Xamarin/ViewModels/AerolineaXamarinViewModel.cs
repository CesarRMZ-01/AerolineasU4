using AerolineasU4_Xamarin.Models;
using AerolineasU4_Xamarin.Services;
using AerolineasU4_Xamarin.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace AerolineasU4_Xamarin.ViewModels
{
    public enum Vistas { TablaVuelo, Agregar, Editar, CancelarVuelo}
    internal class AerolineaXamarinViewModel: INotifyPropertyChanged
    {

        public ObservableCollection<Vuelum> Vuelos { get; set; } = new ObservableCollection<Vuelum>();

        public Vuelum Vuelo { get; set; }
        public string Error { get; set; } = "";

        readonly AerolineasXamarinService service = new AerolineasXamarinService();

        public Command ActualizarCommand { get; set; }

        public Command AgregarCommand { get; set; }
        public Command EditarCommand { get; set; }
        public Command GuardarCommand { get; set; }

        public Command CancelarVueloCommand { get; set; }

        private TimeSpan hora;

        public TimeSpan Hora
        {
            get { return hora; }
            set { 
                    hora = value;
                    Actualizar(nameof(Hora));
                }
        }

        private DateTime fecha = DateTime.Now;

        public DateTime Fecha
        {
            get { return fecha; }
            set { 
                    fecha = value; 
                    Actualizar(nameof(Fecha));
                }
        }

        public Vistas Vista { get; set; } = Vistas.TablaVuelo;

        public AerolineaXamarinViewModel()
        {
            service.Error += Service_Error;
            ActualizarCommand = new Command(CargarVuelos);

            AgregarCommand = new Command(Agregar);
            GuardarCommand = new Command(Guardar);
            EditarCommand = new Command<Vuelum>(Editar);

            CancelarVueloCommand = new Command<Vuelum>(Cancelar);


            CargarVuelos();
        }

        private void Agregar()
        {
            Vuelo = new Vuelum();
            AgregarView agregar = new AgregarView()
            {
                BindingContext = this
            };
            Application.Current.MainPage.Navigation.PushAsync(agregar);
            Error = "";
            Actualizar(nameof(Error));

        }

        private void Service_Error(List<string> obj)
        {
            Error = "";
            obj.ForEach(x => Error += x + "\n");
            Actualizar(nameof(Error));  
        }

        async void CargarVuelos()
        {
            Vuelos.Clear();
            var datos = await service.Get();
            datos.ForEach(x => Vuelos.Add(x));
        }


        void Editar(Vuelum v)
        {
            Error = "";
            Actualizar(nameof(Error));

            Vuelo = new Vuelum
            {
                IdAerolineaU4 = v.IdAerolineaU4,
                Hora = v.Hora,
                Vuelo = v.Vuelo,
                Destino = v.Destino,
                Puerta = v.Puerta,
            };
            fecha = new DateTime(Vuelo.Hora.Year, Vuelo.Hora.Month, Vuelo.Hora.Day);
            hora = new TimeSpan(Vuelo.Hora.Hour, Vuelo.Hora.Minute, Vuelo.Hora.Second);

            EditarView editar = new EditarView() 
            { 
                BindingContext = this 
            };
            Application.Current.MainPage.Navigation.PushAsync(editar);

        }
        async void Guardar()
        {
            Error = "";

            Vuelo.Hora = new DateTime(fecha.Year, fecha.Month, fecha.Day, hora.Hours, hora.Minutes, hora.Seconds);

            if (Vuelo != null)
            {
                if (Validar())
                {
                    if (Vuelo.IdAerolineaU4 == 0)
                    {
                        Vuelo.Estado = "PROGRAMADO";

                        if (await service.Insert(Vuelo))
                        {
                            await Application.Current.MainPage.Navigation.PopAsync();
                        }
                    }
                    else
                    {
                        Vuelo.Estado = "REPROGRAMADO";
                        if (await service.Update(Vuelo))
                        {
                            await Application.Current.MainPage.Navigation.PopAsync();
                        }
                    }
                }
                
                CargarVuelos();

                Actualizar(nameof(Error));
            }
        }
        async void Eliminar()
        {
            if (Vuelo != null)
            {
                Vuelo.Estado = "CANCELADO";
                if (await service.Update(Vuelo))
                {
                    CargarVuelos();
                }
            }
        }

        private async void Cancelar(Vuelum obj)
        {
            bool option = await Application.Current.MainPage.DisplayAlert("Eliminar", "Seguro de Cancelar este vuelo?", "Si", "No");
            if (option == true)
            {
                Vuelo = obj;
                Eliminar();
            }
        }


        bool Validar()
        {
            if (string.IsNullOrWhiteSpace(Vuelo.Destino))
            {
                Error += "El destino no puede estar vacio " + "\n";
            }
            if (Vuelo.Hora < DateTime.Now.AddMinutes(6))
            {
                Error += "Se debe planear el vuelo con 6 minutos de anticipación " + "\n";
            }
            if (string.IsNullOrWhiteSpace(Vuelo.Vuelo))
            {
                Error += "El codigo de vuelo no puede estar vacio " + "\n";
            }
            if (Vuelo.Puerta == null)
            {
                Error += "Selecione una puerta de salida " + "\n";
            }

            return Error == "";
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void Actualizar(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

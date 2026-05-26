using System;
using Microsoft.Maui.Controls;
using Entidades;
using ImbaquingoAaronCine.Services;

namespace ImbaquingoAaronCine;

public partial class EntradasPage : ContentPage
{
    private readonly EntradaService _entradaService;
    private readonly Pelicula _pelicula;
    private const double PrecioBoleto = 8.50;

    public EntradasPage(Pelicula pelicula)
    {
        InitializeComponent();
        _entradaService = new EntradaService();
        _pelicula = pelicula;

        PeliculaTituloLabel.Text = _pelicula.Titulo;
        PeliculaInfoLabel.Text = $"{_pelicula.Duracion} min | Clasificacion: {_pelicula.Clasificacion}";
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarHistorial();
    }

    private void CargarHistorial()
    {
        try
        {
            EntradasCollectionView.ItemsSource = _entradaService.ReadByPeliculaId(_pelicula.Id);
        }
        catch (Exception)
        {
            EntradasCollectionView.ItemsSource = null;
        }
    }

    private void cmdCrearEntrada_Clicked(object sender, EventArgs e)
    {
        try
        {
            int asientos = int.Parse(txtCantidadAsientos.Text);
            double total = asientos * PrecioBoleto;

            DateTime fechaCombinada = pickerFechaFuncion.Date.Value.Add(pickerHoraFuncion.Time.Value);

            _entradaService.Create(
                int.Parse(txtIdEntrada.Text),
                _pelicula.Id,
                asientos,
                fechaCombinada,
                total
            );

            DisplayAlert("Exito", "Entrada guardada de forma correcta.", "OK");
            LimpiarCampos();
            CargarHistorial();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void cmdLeerEntrada_Clicked(object sender, EventArgs e)
    {
        try
        {
            var entrada = _entradaService.ReadById(int.Parse(txtIdEntrada.Text));
            txtCantidadAsientos.Text = entrada.CantidadAsientos.ToString();

            DateTime fechaDb = DateTime.Parse(entrada.FechaFuncion);

            pickerFechaFuncion.Date = (DateTime?)fechaDb.Date;

            pickerHoraFuncion.Time = fechaDb.TimeOfDay;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void cmdActualizarEntrada_Clicked(object sender, EventArgs e)
    {
        try
        {
            int asientos = int.Parse(txtCantidadAsientos.Text);
            double total = asientos * PrecioBoleto;

            // Combinar la fecha del DatePicker con la hora del TimePicker de forma segura
            DateTime fechaRespaldo = pickerFechaFuncion.Date ?? DateTime.Now;
            DateTime fechaCombinada = fechaRespaldo.Date.Add(pickerHoraFuncion.Time.Value);

            _entradaService.Update(
                int.Parse(txtIdEntrada.Text),
                _pelicula.Id,
                asientos,
                fechaCombinada,
                total
            );

            DisplayAlert("Exito", "Entrada actualizada de forma correcta.", "OK");
            LimpiarCampos();
            CargarHistorial();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void cmdBorrarEntrada_Clicked(object sender, EventArgs e)
    {
        try
        {
            _entradaService.Delete(int.Parse(txtIdEntrada.Text));
            DisplayAlert("Exito", "Entrada eliminada de forma correcta.", "OK");
            LimpiarCampos();
            CargarHistorial();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void LimpiarCampos()
    {
        txtIdEntrada.Text = string.Empty;
        txtCantidadAsientos.Text = string.Empty;
        pickerFechaFuncion.Date = DateTime.Now;
        pickerHoraFuncion.Time = DateTime.Now.TimeOfDay;
    }
}
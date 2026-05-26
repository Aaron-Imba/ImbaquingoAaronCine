using System;
using System.Linq;
using Microsoft.Maui.Controls;
using Entidades;
using ImbaquingoAaronCine.Services;

namespace ImbaquingoAaronCine;

public partial class PeliculasPage : ContentPage
{
    private readonly PeliculaService _peliculaService;

    public PeliculasPage()
    {
        InitializeComponent();
        _peliculaService = new PeliculaService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarLista();
    }

    private void CargarLista()
    {
        try
        {
            PeliculasCollectionView.ItemsSource = _peliculaService.ReadAll();
        }
        catch (Exception)
        {
            PeliculasCollectionView.ItemsSource = null;
        }
    }

    private void cmdCrear_Clicked(object sender, EventArgs e)
    {
        try
        {
            _peliculaService.Create(
                int.Parse(txtId.Text),
                txtTitulo.Text,
                int.Parse(txtDuracion.Text),
                txtClasificacion.Text
            );
            DisplayAlert("Exito", "Pelicula creada exitosamente.", "OK");
            LimpiarCampos();
            CargarLista();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void cmdLeer_Clicked(object sender, EventArgs e)
    {
        try
        {
            var p = _peliculaService.ReadById(int.Parse(txtId.Text));
            txtTitulo.Text = p.Titulo;
            txtDuracion.Text = p.Duracion.ToString();
            txtClasificacion.Text = p.Clasificacion;
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void cmdActualizar_Clicked(object sender, EventArgs e)
    {
        try
        {
            _peliculaService.Update(
                int.Parse(txtId.Text),
                txtTitulo.Text,
                int.Parse(txtDuracion.Text),
                txtClasificacion.Text
            );
            DisplayAlert("Exito", "Pelicula actualizada exitosamente.", "OK");
            LimpiarCampos();
            CargarLista();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void cmdBorrar_Clicked(object sender, EventArgs e)
    {
        try
        {
            _peliculaService.Delete(int.Parse(txtId.Text));
            DisplayAlert("Exito", "Pelicula eliminada exitosamente.", "OK");
            LimpiarCampos();
            CargarLista();
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void OnPeliculaSeleccionada(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Pelicula p) return;
        PeliculasCollectionView.SelectedItem = null;
        await Shell.Current.Navigation.PushAsync(new EntradasPage(p));
    }

    private void LimpiarCampos()
    {
        txtId.Text = string.Empty;
        txtTitulo.Text = string.Empty;
        txtDuracion.Text = string.Empty;
        txtClasificacion.Text = string.Empty;
    }
}
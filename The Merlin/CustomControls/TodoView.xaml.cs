using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;

namespace The_Merlin.CustomControls;

public partial class TodoView : ContentView
{
    public static readonly BindableProperty myTodoItemProperty = BindableProperty.Create(nameof(myTodoItem), typeof(TodoItem), typeof(TodoView));
    public TodoItem myTodoItem { get => (TodoItem)GetValue(myTodoItemProperty); set => SetValue(myTodoItemProperty, value); }

    public static readonly BindableProperty NavigateCommandProperty = BindableProperty.Create(nameof(NavigateCommand), typeof(ICommand), typeof(TodoView));
    public ICommand NavigateCommand { get => (ICommand)GetValue(NavigateCommandProperty); set => SetValue(NavigateCommandProperty, value); }
    
    public TodoView()
	{
		InitializeComponent();
    }
}
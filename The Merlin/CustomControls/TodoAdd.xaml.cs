using System.Windows.Input;
using The_Merlin.Data;
using The_Merlin.Models;
using The_Merlin.ViewModels;

namespace The_Merlin.CustomControls;

public partial class TodoAdd : ContentView
{
    public static readonly BindableProperty TodoDefsProperty = BindableProperty.Create(nameof(TodoDefs), typeof(IEnumerable<TodoDefItem>), typeof(TodoAdd));
    public IEnumerable<TodoDefItem> TodoDefs { get => (IEnumerable<TodoDefItem>)GetValue(TodoDefsProperty); set => SetValue(TodoDefsProperty, value); }

    public static readonly BindableProperty AddCommandProperty = BindableProperty.Create(nameof(AddCommand), typeof(ICommand), typeof(TodoAdd));
    public ICommand AddCommand { get => (ICommand)GetValue(AddCommandProperty); set => SetValue(AddCommandProperty, value); }

    public static readonly BindableProperty SelectedTodoDefProperty = BindableProperty.Create(nameof(SelectedTodoDef), typeof(TodoDefItem), typeof(TodoAdd), default(TodoDefItem), defaultBindingMode: BindingMode.TwoWay);
    public TodoDefItem SelectedTodoDef { get => (TodoDefItem)GetValue(SelectedTodoDefProperty); set { SetValue(SelectedTodoDefProperty, value); } }

    public TodoAdd()
    {
        InitializeComponent();
    }
}
using System;

namespace UI
{
    public interface IView
    {
        Type ViewModelType { get; } 
        void Bind(IViewModel model);
    }
}
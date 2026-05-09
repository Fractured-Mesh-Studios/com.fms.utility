using UnityEngine;

namespace UtilityEngine
{
    /// <summary>
    /// Attribute para mostrar un dropdown con los estados disponibles de un Animator en el inspector.
    /// Use en propiedades string para seleccionar estados de animación por nombre.
    /// </summary>
    public class AnimatorStateAttribute : PropertyAttribute
    {
        public string animatorFieldName;

        /// <summary>
        /// Constructor del atributo.
        /// </summary>
        /// <param name="animatorFieldName">Nombre del campo que contiene el componente Animator (ej: "animator", "_animator")</param>
        public AnimatorStateAttribute(string animatorFieldName = "animator")
        {
            this.animatorFieldName = animatorFieldName;
        }
    }
}

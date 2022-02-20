using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public abstract class DefinableAttributePreview : BaseDefinablePreview
    {
        [Header("Attribute Refs.")]
        [ReadOnlyInspector] public LevelingHub _levelingHub;
        [ReadOnlyInspector] public bool _isAcceptButton;

        /// When player pressed Right Arrow to increment this attribute.
        public abstract void IncrementAttributePreview();

        protected abstract void RedrawIncrementPreview();

        /// When player pressed Left Arrow to decrement this attribute.
        public abstract void DecrementAttributePreview();

        protected abstract void RedrawDecrementPreview(bool _hasBackToInitialValue);

        /// When menu opened, when player confirmed the result.
        public abstract void RedrawConfirmedAttributePreview();

        /// Must be called in any attributes type, either included highlighter setup or not
        public abstract void AttributePreviewSetup(LevelingHub _levelingHub);
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;

#if WinUI
using Microsoft.UI.Xaml;
#else
using Windows.UI.Xaml;
#endif

namespace Uno.Material
{
	/// <summary>
	/// Material resources including colors, layout values and styles
	/// </summary>
	public sealed class MaterialResourcesV2 : ResourceDictionary
	{
		private const string StylePrefix = "M3Material";
		private ResourceDictionary _implicitResources = new ResourceDictionary();

		public MaterialResourcesV2()
		{
			Source = new Uri("ms-appx:///Uno.Material/Generated/mergedpages.v2.xaml");

			MapStyleInfo();
		}

		public bool WithImplicitStyles { set => ExportImplicitStyles(value); }

		private void MapStyleInfo()
		{
			var aliasedResources = new ResourceDictionary();
			foreach (var (resKey, sharedKey, isDefaultStyle) in GetStyleInfos())
			{
				var style = GetStyle(resKey);

				if (isDefaultStyle)
				{
					_implicitResources.Add(style.TargetType, style);
				}

				aliasedResources.Add(sharedKey, style);
			}

			// UWP don't allow for res-dict with Source set to contain resource directly:
			// > Local values are not allowed in resource dictionary with Source set
			// but, we can add them through merged-dict instead.
			this.MergedDictionaries.Add(aliasedResources);
		}

		private void ExportImplicitStyles(bool value)
		{
			if (!value) return; // we don't support teardown

			this.MergedDictionaries.Add(_implicitResources);
		}

		private Style GetStyle(string key)
		{
			if (!this.TryGetValue(key, out var resource) || !(resource is Style style))
			{
				// uwp: If the {key} style is clearly defined, but we can't find it here.
				// And, that it only happens on uwp, and not other uno platforms.
				// It means that the style references resources that are not directly included.
				// note: Resources used on Style.Setters need to be directly defined/included, those used in Style.Template dont have to be.
				throw new ArgumentException($"Missing resource: key={key}");
			}
			if (style.TargetType == null)
			{
				throw new InvalidOperationException($"Missing TargetType on style: key={key}");
			}

			return style;
		}

		private IEnumerable<(string ResourceKey, string SharedKey, bool IsDefaultStyle)> GetStyleInfos()
		{
			var result = new List<(string ResourceKey, string SharedKey, bool IsDefaultStyle)>();

			Add("M3MaterialCheckBoxStyle", isImplicit: true);
			Add("M3MaterialAppBarButtonStyle");
			Add("M3MaterialCommandBarStyle");
			Add("M3MaterialRadioButtonStyle", isImplicit: true);
			Add("M3MaterialDisplayLarge");
			Add("M3MaterialDisplayMedium");
			Add("M3MaterialDisplaySmall");
			Add("M3MaterialHeadlineLarge");
			Add("M3MaterialHeadlineMedium");
			Add("M3MaterialHeadlineSmall");
			Add("M3MaterialTitleLarge");
			Add("M3MaterialTitleMedium");
			Add("M3MaterialTitleSmall");
			Add("M3MaterialLabelLarge");
			Add("M3MaterialLabelMedium");
			Add("M3MaterialLabelSmall");
			Add("M3MaterialBodyLarge");
			Add("M3MaterialBodyMedium", isImplicit: true);
			Add("M3MaterialBodySmall");
			Add("M3MaterialOutlinedTextBoxStyle");
			Add("M3MaterialFilledTextBoxStyle", isImplicit: true);
			Add("M3MaterialOutlinedPasswordBoxStyle");
			Add("M3MaterialFilledPasswordBoxStyle", isImplicit: true);
			Add("M3MaterialElevatedButtonStyle");
			Add("M3MaterialFilledButtonStyle", isImplicit: true);
			Add("M3MaterialFilledTonalButtonStyle");
			Add("M3MaterialOutlinedButtonStyle");
			Add("M3MaterialTextButtonStyle");
			Add("M3MaterialIconButtonStyle");

			// ** TODO **

			//Add("M3MaterialIconToggleButtonStyle");
			//Add("M3MaterialToggleButtonStyle");
			//Add("M3MaterialMediumFabButtonStyle");
			//Add("M3MaterialLargeFabButtonStyle");
			//Add("M3MaterialElevatedInputChipStyle");
			//Add("M3MaterialElevatedAssistChipStyle");
			//Add("M3MaterialElevatedFilterChipStyle");
			//Add("M3MaterialElevatedSuggestionChipStyle");
			//Add("M3MaterialOutlinedInputChipStyle");
			//Add("M3MaterialOutlinedAssistChipStyle");
			//Add("M3MaterialOutlinedFilterChipStyle");
			//Add("M3MaterialOutlinedSuggestionChipStyle");
			//Add("M3MaterialElevatedInputChipGroupStyle");
			//Add("M3MaterialElevatedAssistChipGroupStyle");
			//Add("M3MaterialElevatedFilterChipGroupStyle");
			//Add("M3MaterialElevatedSuggestionChipGroupStyle");
			//Add("M3MaterialOutlinedInputChipGroupStyle");
			//Add("M3MaterialOutlinedAssistChipGroupStyle");
			//Add("M3MaterialOutlinedFilterChipGroupStyle");
			//Add("M3MaterialOutlinedSuggestionChipGroupStyle");
			//Add("M3MaterialSliderStyle");
			//Add("M3MaterialToggleSwitchStyle");
			//Add("M3MaterialElevatedCardContentControlStyle");
			//Add("M3MaterialFilledCardContentControlStyle");
			//Add("M3MaterialOutlinedCardContentControlStyle");
			//Add("M3MaterialFlyoutPresenterStyle");
			//Add("M3MaterialDatePickerStyle");
			//Add("M3MaterialTimePickerStyle");
			//Add("M3MaterialCalendarDatePickerStyle");
			//Add("M3MaterialListViewStyle");
			//Add("M3MaterialListViewItemStyle");
			//Add("M3MaterialInfoBarStyle");
			//Add("M3MaterialProgressRingStyle");
			//Add("M3MaterialProgressBarStyle");

			return result;

			void Add(string key, string alias = null, bool isImplicit = false) =>
				result.Add((key, alias ?? key.Substring(StylePrefix.Length), isImplicit));
		}
	}
}

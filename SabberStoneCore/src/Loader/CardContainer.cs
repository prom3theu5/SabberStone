﻿#region copyright
// SabberStone, Hearthstone Simulator in C# .NET Core
// Copyright (C) 2017-2019 SabberStone Team, darkfriend77 & rnilva
//
// SabberStone is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License.
// SabberStone is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using Newtonsoft.Json;
using SabberStoneCore.Model;
using SabberStoneCore.src.Loader;
//using SabberStoneCore.Properties;

namespace SabberStoneCore.Loader
{
	internal class CardContainer : IEnumerable<Card>
	{
		/// <summary>The cards container</summary>
		/// <autogeneratedoc />
		internal Dictionary<string, Card> Cards { get; private set; }

		/// <summary>Gets the <see cref="Card"/> with the specified card identifier.</summary>
		/// <value>The <see cref="Card"/>.</value>
		/// <param name="cardId">The card identifier.</param>
		/// <returns></returns>
		/// <autogeneratedoc />
		internal Card this[string cardId] => Cards[cardId];

		internal void Load(IEnumerable<Card> cards)
		{
			// Set cards (without behaviours)
			Cards = (from c in cards select new { Key = c.Id, Value = c }).ToDictionary(x => x.Key, x => x.Value);

			// Add Powers
			foreach (Card c in Cards.Values)
			{
				if (CardDefs.Instance.Get.TryGetValue(c.Id, out CardDef cardDef))
				{
					// test current PlayReq against last cardDef with playreq CardDefs-36393.xml
					if (c.PlayRequirements != null)
					{
						foreach (KeyValuePair<Enums.PlayReq, int> keyValuePair in c.PlayRequirements)
						{
							if (cardDef.PlayReqs == null)
							{
								Console.WriteLine($"{c.Id} missing {keyValuePair.Key} = {keyValuePair.Value}!!!");
							}
							else if (!cardDef.PlayReqs.Any(p => p.Key == keyValuePair.Key && p.Value == keyValuePair.Value))
							{
								Console.WriteLine($"{c.Id} missing {keyValuePair.Key} = {keyValuePair.Value}!!!");
							}
						}
					}

					c.Power = cardDef.Power;
					c.Implemented = cardDef.Power == null ||
									cardDef.Power.PowerTask != null ||
									cardDef.Power.DeathrattleTask != null ||
									cardDef.Power.ComboTask != null ||
									cardDef.Power.TopdeckTask != null ||
									cardDef.Power.OverkillTask != null ||
									cardDef.Power.Aura != null ||
									cardDef.Power.Trigger != null ||
									cardDef.Power.Enchant != null;
				}
				else
				{
					//if (c.PlayRequirements != null && c.PlayRequirements.Count > 0)
					//{
					//	Console.WriteLine($"{c.Id} missing {c.PlayRequirements}!!!");
					//}
				}
			}
		}

		/// <summary>Returns an enumerator that iterates through the collection.</summary>
		/// <returns>An enumerator that can be used to iterate through the collection.</returns>
		/// <autogeneratedoc />
		public IEnumerator<Card> GetEnumerator()
		{
			return Cards.Values.GetEnumerator();
		}

		/// <summary>Returns an enumerator that iterates through a collection.</summary>
		/// <returns>
		/// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
		/// </returns>
		/// <autogeneratedoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

	}
}

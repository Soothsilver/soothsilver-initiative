using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using PostSharp.Patterns.Recording;
using PostSharp.Patterns.Model;
using PostSharp.Patterns.Collections;
using System.Linq;

namespace Sit
{
    [Recordable]
    public class Encounter
    {
        private static string CREATURE_SEPARATOR = "[[CREATURE-SEPARATOR]]";
        [Reference]
        private Creature _activeCreature = null;

        public Creature ActiveCreature
        {
            get { return _activeCreature; }
            set
            {
                Creature before = _activeCreature;
                _activeCreature = value;
                Creature after = _activeCreature;
                before?.OnPropertyChanged(nameof(Creature.Active));
                after?.OnPropertyChanged(nameof(Creature.Active));
            }
        }


        [Pure]
        public string Serialize()
        {
            return string.Join(CREATURE_SEPARATOR, Creatures.Where(cr => cr.Friendly).Select(cr => cr.Name));
        }

        [Pure]
        public void Deserialize(string savedEncounter)
        {
            string[] crs = savedEncounter.Split(new string[] { CREATURE_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var cr in crs)
            {
                AddCreature(new Creature(cr, true));
            }
        }

        [Child(ItemsRelationship = RelationshipKind.Child)]
        public AdvisableCollection<Creature> Creatures = new AdvisableCollection<Creature>();

        public void AfterManualInitiatives()
        {
            foreach(var cr in Creatures)
            {
                if (cr.TargetInitiative != int.MinValue && cr.TargetInitiative != 0)
                {
                    cr.Initiative = cr.TargetInitiative;
                }
            }
            Creatures.MySort(cr => -cr.Initiative);
        }

        [ThenSave]
        public void Kill(Creature creature)
        {
            Creatures.Remove(creature);
        }
        public void Up(Creature creature)
        {
            int myIndex = Creatures.IndexOf(creature);
            if (myIndex > 0)
            {
                Creatures.RemoveAt(myIndex);
                Creatures.Insert(myIndex - 1, creature);
            }
        }
        public void Down(Creature creature)
        {
            int myIndex = Creatures.IndexOf(creature);
            if (myIndex < Creatures.Count - 1)
            {
                Creatures.RemoveAt(myIndex);
                Creatures.Insert(myIndex + 1, creature);
            }
        }

        [ThenSave]
        public void AddCreature(Creature creature)
        {
            if (Creatures.Count == 0)
            {
                ActiveCreature = creature;
            }
            Creatures.Add(creature);
        }

        public void MoveInitiative(int delta)
        {
            if (ActiveCreature == null)
            {
                if (Creatures.Count > 0)
                {
                    ActiveCreature = Creatures[0];
                }
                return;
            }
            int thisInitiative = Creatures.IndexOf(ActiveCreature);
            if (thisInitiative == -1)
            {
                return;
            }
            int newInitiative = thisInitiative + delta;
            if (newInitiative < 0)
            {
                newInitiative = Creatures.Count - 1;
            }
            if (newInitiative >= Creatures.Count)
            {
                newInitiative = 0;
            }
            ActiveCreature = Creatures[newInitiative];
        }
    }
}

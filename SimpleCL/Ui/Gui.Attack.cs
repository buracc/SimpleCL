using System;
using SimpleCL.Interaction;
using SimpleCL.Models;
using SimpleCL.Models.Skills;

namespace SimpleCL.Ui
{
    partial class Gui
    {
        private void AddSkill(object sender, EventArgs e)
        {
            var selected = availSkillsListBox.SelectedItem;
            if (selected is not CharacterSkill characterSkill)
            {
                return;
            }

            SelectedSkills.Add(characterSkill);
        }

        private void RemoveSkill(object sender, EventArgs e)
        {
            var selected = attackSkillsListBox.SelectedItem;
            if (selected is not CharacterSkill characterSkill)
            {
                return;
            }

            SelectedSkills.Remove(characterSkill);
        }

        private void AddEntity(object sender, EventArgs e)
        {
            var selected = nearEntitiesListBox.SelectedItem;
            if (selected is not ITargetable target)
            {
                return;
            }

            SelectedEntities.Add(target);
        }

        private void RemoveEntity(object sender, EventArgs e)
        {
            var selected = attackEntitiesListBox.SelectedItem;
            if (selected is not ITargetable target)
            {
                return;
            }

            SelectedEntities.Remove(target);
        }

        private void StartAttack(object sender, EventArgs e)
        {
            InteractionQueue.Get.StartLoop();
        }
    }
}
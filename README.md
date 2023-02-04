# WorkshopOptimizerPlugin
FFXIV Island Sanctuary Workshop Optimizer Plugin

# Installation

1. /xlsettings -> Experimental tab
2. Add https://raw.githubusercontent.com/belzaru17/WorkshopOptimizerPlugin/main/repo.json to the Custom Plugin Repositories
3. Click on the + button
4. Click on the "Save and Close" button
5. /xlplugins -> Enable WorkshopOptimizerPlugin

# Usage

1. Go to your island
2. Talk to the Tactful Taskmaster NPC and open the workshop agenda
3. Open "Review Supply & Demand" window
4. Type /wso
5. Select the cycle you want to optimize and select the items you want to craft
6. Set those items (now in Produced tab) to the in-game workshop agenda
7. Profit!

NOTE: Predictions on the 1st day of the cycle are not 100% reliable.  You need to do the above during days 1-4 of the cycle to get accurate predictions.

# Tabs

## Workshops
Shows possible combinations of items using all 3 workshows and expected values given Supply/Demand patterns.  Allows you to select which items you'll produce.  These items also need to be set in-game, but will be used by the plugin when calculating future cycle options and profits.

## Produced
Shows the selected items to be produced.  Selecting items here only has effect within the plugin, it doesn't change your workshop agenda.

## Next Week
Shows rare materials from the Granery that will popular next week.

## Items
This tab shows information about all items.  Supply/Demand with an * means it's a prediction.
It also allows you to specify which items to use and when.

## Patterns
Shows predictedicted patterns of Supply/Demand for items for the selected cycle.  Each pattern is represented by CxW or CxS, with x being the cycle in which the itema will peak.  W represents a Weak Pattern (Supply will be Insufficient) and S represents a Strong Pattern (Supply will be Nonexistent).

## Combinations
Shows possible combinations of items and expected values given Supply/Demand patterns.  Allows you to select which items you'll produce.  These items also need to be set in-game, but will be used by the plugin when calculating future cycle options and profits.

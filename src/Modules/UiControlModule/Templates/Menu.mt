<component>
    <using namespace="System.Linq"/>
    <using namespace="System.Collections.Generic"/>

    <import component="EvoSC.Containers.Window" as="Window"/>
    <import component="EvoSC.Style.UIStyle" as="UIStyle"/>
    <import component="EvoSC.Controls.Button" as="Button"/>
    <import component="EvoSC.Controls.Checkbox" as="Checkbox"/>
    <import component="EvoSC.HiddenEntry" as="HiddenEntry" />

    <property type="List<string>" name="moduleNames"/>
    <property type="List<string>" name="hiddenModules"/>

    <template>
        <UIStyle/>

        <Window
                width="120"
                height="90"
                x="-30"
                y="25"
                title="UI Control"
                icon=""
        >
            <frame id="checkboxList">
                <Checkbox
                        id="checkbox_{{ __index }}"
                        foreach="string moduleName in moduleNames"
                        y="{{ __index * -6 }}"
                        isChecked='{{ hiddenModules.Contains(moduleName) }}'
                        text="{{ moduleName }}"
                />
            </frame>
            
            <HiddenEntry
                    id="hiddenManialinksEntry"
                    name="HiddenManialinks"
            />
            
            <Button id="btnSave" text="Submit" action="UiControlModule/SaveConfiguration" x="90" y="-42"/>
        </Window>
    </template>

    <script><!--
        Void AttachDataToCheckboxes() {
            declare checkboxList <=> (Page.MainFrame.GetFirstChild("checkboxList") as CMlFrame);
            declare Text[] moduleNames = [{! string.Join(",", moduleNames.Select(name => "\u0022" + name + "\u0022")) !}];
            declare moduleIndex = 0;
            
            foreach(name in moduleNames){
                declare Control <=> checkboxList.Controls[moduleIndex];
                declare Text moduleName for Control = name;
                moduleIndex += 1;
            }
        }
    
        *** OnInitialization ***
        ***
            AttachDataToCheckboxes();
            declare Text[] hiddenModules = [{! string.Join(",", hiddenModules.Select(name => "\u0022" + name + "\u0022")) !}];
            declare CMlEntry hiddenManialinksEntry <=> (Page.MainFrame.GetFirstChild("hiddenManialinksEntry") as CMlEntry);
        ***
        
        *** OnCheckboxToggle ***
        ***
            declare Text moduleName for Control = "";
            declare Boolean showModule = !IsChecked;
            
            if(showModule){
                if(hiddenModules.exists(moduleName)){
                    hiddenModules.remove(moduleName);
                }
            }else{
                hiddenModules.add(moduleName);
            }
            
            hiddenManialinksEntry.Value = TextLib::Join("|", hiddenModules);
        ***
    --></script>

    <script resource="EvoSC.Scripts.UIScripts"/>
</component>

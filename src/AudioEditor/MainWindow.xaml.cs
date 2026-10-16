<Window x:Class="AudioEditor.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Audio Editor"
        Height="760"
        Width="1100"
        MinHeight="500"
        MinWidth="780"
        WindowStartupLocation="CenterScreen"
        KeyDown="Window_KeyDown">
    <Grid Margin="12">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <ToolBar Grid.Row="0" Padding="8" Margin="0,0,0,8">
            <Button Name="OpenButton" Click="OpenButton_Click" Padding="10,6" AutomationProperties.Name="Open audio file" ToolTip="Open audio file">Open</Button>
            <Button Name="PlayPauseButton" Click="PlayPauseButton_Click" Padding="10,6" Margin="6,0,0,0" AutomationProperties.Name="Play or pause audio" ToolTip="Play or pause audio">Play</Button>
            <Button Name="StopButton" Click="StopButton_Click" Padding="10,6" Margin="6,0,0,0" AutomationProperties.Name="Stop playback" ToolTip="Stop playback">Stop</Button>
            <Button Name="TrimExportButton" Click="TrimExportButton_Click" Padding="10,6" Margin="6,0,0,0" AutomationProperties.Name="Trim selection and export to WAV" ToolTip="Trim selected section and export as WAV">Trim &amp; Export</Button>
        </ToolBar>

        <Border Grid.Row="1" BorderBrush="#C8C8C8" BorderThickness="1" Background="#FAFAFA" Padding="10" Margin="0,0,0,8">
            <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                <TextBlock Text="Selection:" FontWeight="Bold" VerticalAlignment="Center" Margin="0,0,8,0"/>
                <TextBox Name="SelectionStartText" Width="90" Margin="0,0,8,0" AutomationProperties.Name="Selection start in seconds" ToolTip="Selection start in seconds"/>
                <TextBlock Text="to" VerticalAlignment="Center" Margin="0,0,8,0"/>
                <TextBox Name="SelectionEndText" Width="90" Margin="0,0,12,0" AutomationProperties.Name="Selection end in seconds" ToolTip="Selection end in seconds"/>
                <TextBlock Text="seconds" VerticalAlignment="Center" Margin="0,0,16,0"/>
                <TextBlock Name="FileInfoText" FontWeight="Bold" VerticalAlignment="Center" AutomationProperties.Name="Opened file information" Text="No file loaded"/>
            </StackPanel>
        </Border>

        <Border Grid.Row="2" BorderBrush="#B8B8B8" BorderThickness="1" Background="#FFFFFF" Padding="10">
            <Grid>
                <Image Name="WaveformImage" Stretch="Fill" VerticalAlignment="Stretch" HorizontalAlignment="Stretch" AutomationProperties.Name="Audio waveform display"/>
                <TextBlock Text="Waveform" Foreground="#707070" FontSize="12" Margin="10,6,0,0" IsHitTestVisible="False"/>
            </Grid>
        </Border>

        <StatusBar Grid.Row="3" Margin="0,8,0,0" Padding="8,4">
            <StatusBarItem>
                <TextBlock Name="StatusText" Text="Ready" AutomationProperties.Name="Status message"/>
            </StatusBarItem>
        </StatusBar>
    </Grid>
</Window>

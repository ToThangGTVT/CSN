   M S B A M L   `   `   `  ���� [  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  K GAxInterop.WMPLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=0clr-namespace:AxWMPLib;assembly=AxInterop.WMPLibAxWMPLib  CSN   CSN.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     XwindowForms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     KactiveXControl0clr-namespace:AxWMPLib;assembly=AxInterop.WMPLib     -     �Name$  frm_main��$��399.667��$	��554�� � Title ��  �� Path$ Content� �� ElementName$ txt_sus��.��� -   $��0,20,0,1q� � RowDefinitions ��  � ColumnDefinitions �� �� $
��515* ��� .+��� -    ��Name$	 rtb�� �� 
IsReadOnly . $��259,57,13,76q�{�. $��1��. �/� �� $��Lời bài hát��$��249,34,4,65q�$��1���� -   $��Tìm kiếm�� �� 	IsDefault . $��23��$��0,6,4,0q�$	��Top=�$��Right=�$��83��$��1���� -   	 ��Name$
	 scrl��$��26,0,338,10q�$��23��$��Bottom=�
 � SmallChange$
 1Q�$
��Auto=�$
��Auto=� �� IsMoveToPointEnabled . $��1���� -    ��Name$ txttotal��$��00:00��$��	0,0,13,36q�$��29��$��Bottom=�$��Right=�$��51��$��1���� -   $ txtreal��$��00:00��$
��Left=�$��	26,0,0,33q�$��39��$��32��$��Bottom=�$��1���� -    ��Name$
 txts��$��23��$��5,6,92,0q� �� TextWrapping$
 Wrap=�$	��Top=�$��1������ $
 Text�$ 	cbo_input���� -	   $ txt_sus��$	��CSN��$
��Left=�$��302,272,0,23q�$��41�� J� Opacity$ 0Q�$��1��7  1System.Windows.Forms.Integration.WindowsFormsHost  -
     Name$ winform��$��44��$��Bottom=�$��3��$��
-1,0,-1,-2q�   Child. #  AxWMPLib.AxWindowsMediaPlayer  -   wmp��"   System.Windows.Forms.Control   Visible . �� -   $ txttime��$��Label��$
��Left=�$��24��$��209,272,0,0q�$	��Top=�$��59��$ 0Q�$��1���� -    ��Name$	 chk��$��Repeat��$��1��$��	70,0,0,36q�$��23��$��Bottom=�$
��Left=�$��58��o� $��	0,-20,0,0q�$��3��$��20��$	��Top=�.@�m� $��_File��.@�m� $��_Exit��m� $��Player��.@�m� $
��Play��m� $��Pause��m� $��Repeat���� m� -   $��Delete history���� m� -   $��Option��m� $
��Help��.@�m� -   $��	Home page��m� -   $��Check for update��m� -   $��Donate���� m� -   $��About...���� -    ��Name$ 	cbo_input��$��1��$��23��$��5,6,92,0q�$	��Top=� �� 
IsEditable . �� -    ��Name$ tab_control��$��1��$
��Left=�$��	5,34,0,68q�$	��239�� �� SelectedIndex$ 0��.@��� -    ��Name$ tab_BXH��$��Top BXH��.��� .+��� -    ��Name$ lst_BXH��$��5,5,0,7q�$
��Left=�$	��222���� -   $ tab_timkiem��$��Tìm kiếm��.���� -   $	 lst��$��5,5,0,7q�$
��Left=�$	��222���� -   $ 
tab_dexuat��$��Đề xuất��.���� -   $ 
lst_dexuat��$��5,5,0,7q�$
��Left=�$	��222��
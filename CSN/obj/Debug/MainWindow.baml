   M S B A M L   `   `   `  ����[  WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms  5      K GAxInterop.WMPLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=0clr-namespace:AxWMPLib;assembly=AxInterop.WMPLibAxWMPLib 5       CSN   CSN.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      XwindowForms@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5      KactiveXControl0clr-namespace:AxWMPLib;assembly=AxInterop.WMPLib     5      -   5          �Name$  frm_main��5   	   $��399.667��5   9   $	��554��6J    � Title 6   ��  �� Path$ Content� �� ElementName$ txt_sus��.��5      � -   5        $��0,20,0,1q�5        � RowDefinitions 5   
   �� 5	       � ColumnDefinitions 5   
   �� 5      �� 5      $
��515* �6   �� 5      .+�5   
   �� -   5         ��Name$	 rtb��5       �� 
IsReadOnly . $��259,57,13,76q�65   {�. $��1��6\   . �5      /� 5      �� 5   
   $��Lời bài hát��6   $��249,34,4,65q�6(   $��1��6=   �� 5   
   -   5        $��Tìm kiếm��5       �� 	IsDefault . $��23��65   $��0,6,4,0q�6A   $	��Top=�6R   $��Right=�6j   $��83��6�   $��1��6�   �� 5   
   -   5        	 ��Name$
	 scrl��5      $��26,0,338,10q�6   $��23��64   $��Bottom=�6@   
 � SmallChange$
 1Q�6[   $
��Auto=�6k   $
��Auto=�6�    �� IsMoveToPointEnabled . $��1��6�   �� 5   
   -   5         ��Name$ txttotal��5      $��00:00��6"   $��	0,0,13,36q�62   $��29��6E   $��Bottom=�6Q   $��Right=�6l   $��51��6�   $��1��6�   �� 5   
   -   5        $ txtreal��5      $��00:00��6!   $
��Left=�61   $��	26,0,0,33q�6L   $��39��6_   $��32��6j   $��Bottom=�6v   $��1��6�   �� 5   
   -   5         ��Name$
 txts��5      $��23��6R   $��5,6,92,0q�6^    �� TextWrapping$
 Wrap=�6p   $	��Top=�6�   $��1��6�   ��6    �� $
 Text�$ 	cbo_input��6�   �� 5   
   -	   5        $ txt_sus��5      $	��CSN��6!   $
��Left=�6/   $��302,272,0,23q�6J   $��41��6`    J� Opacity$ 0Q�6k   $��1��6w   7  1System.Windows.Forms.Integration.WindowsFormsHost  5   
   -
   5          Name$ winform��5      $��44��6,   $��Bottom=�68   $��3��6S   $��
-1,0,-1,-2q�6g      Child. 5      #  AxWMPLib.AxWindowsMediaPlayer  -   5        wmp��"   System.Windows.Forms.Control   Visible . 5   ?   5      �� 5   
   -   5        $ txttime��5      $��Label��6!   $
��Left=�61   $��24��6L   $��209,272,0,0q�6X   $	��Top=�6m   $��59��6�   $ 0Q�6�   $��1��6�   �� 5   
   -   5         ��Name$	 chk��5      $��Repeat��6    $��1��61   $��	70,0,0,36q�6A   $��23��6T   $��Bottom=�6`   $
��Left=�6{   $��58��6�   o� 5    
   $��	0,-20,0,0q�6   $��3��6"   $��20��66   $	��Top=�6B   .@�5!      m� $��_File��6   .@�5"      m� $��_Exit��6   5#      m� 5$      $��Player��6   .@�5%      m� $
��Play��6   m� 5&      $��Pause��6   m� 5'      $��Repeat��6   �� 5(      m� 5)      -   5        $��Delete history��5)      63   �� 5*      m� 5+      -   5        $��Option��5+      6+   5,      m� 5-      $
��Help��6   .@�5.      m� -   5        $��	Home page��5.      6.   m� 5/      -   5        $��Check for update��5/      65   m� 50      -   5        $��Donate��50      6+   �� 51      m� 52      -   5        $��About...��52      6-   53      54      �� 55   
   -   5         ��Name$ 	cbo_input��55      $��1��6'   $��23��67   $��5,6,92,0q�6C   $	��Top=�6U    �� 
IsEditable . 6m   �� 56   
   -   5         ��Name$ tab_control��56      $��1��6*   $
��Left=�6:   $��	5,34,0,68q�6U   $	��239��6h    �� SelectedIndex$ 0��6t   .@�57      �� -   5         ��Name$ tab_BXH��57      $��Top BXH��6'   .��58      � .+�59      �� -   5         ��Name$ lst_BXH��59      $��5,5,0,7q�6/   $
��Left=�6@   $	��222��6[   5:      5;      �� 5<      -   5        $ tab_timkiem��5<      $��Tìm kiếm��6+   .��5=      �� -   5        $	 lst��5=      $��5,5,0,7q�6'   $
��Left=�68   $	��222��6S   5>      �� 5?      -   5        $ 
tab_dexuat��5?      $��Đề xuất��6*   .��5@      �� -   5        $ 
lst_dexuat��5@      $��5,5,0,7q�6.   $
��Left=�6?   $	��222��6Z   5A      5B      5C      5D      
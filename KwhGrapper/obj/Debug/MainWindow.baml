   M S B A M L   `   `   `  ����E  AKwhGrapper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:KwhGrapper
KwhGrapper  5   	       KwhGrapper.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:KwhGrapper     5   	     � 
ResizeMode$  CanMinimize=�5       � Title$ Kwh网站爬虫��5   	   $	��350��6   $	��525��6&    � 	Resources 5	       �� 
TargetType �� Property �� Value�� 5
   
   %�   '��       '�� w     �� 5      " ��6   .�5      ��  � ��$	 5,0q�6+   69   �� 5       � ��$ 80��6*   67   5      �� 5      " ��6   .�5      ��  � ��$	 5,0q�6+   69   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $��50 �6   6*   �� 5      $��* �6   6)   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��Center=�6.   .+�5      �� .��6   当前状态：6   �� 5      -   5         ��Name$ lb_state��5      .��6&   	未启动6+   �� 5      -   5        	 ��Name$	 	btn_start��5      .��6@   启动6D   �� 5      -   5        $	 btn_stop��5      {�.  .��6P   停止6T   5      �� 5   
   $��1��6   $ 
Horizontal=�6"   .+�5       �� $y�	Collapsed=�6   .+�5!      �� .��6   邮件名单列表：6!   �� 5"      -   5        
 ��Name$
 lbx_sendNames��5"      $	��160��61   $	��130��6=   6L   5#      �� 5$      .+�5%      �� .��6   运行日志：6   �� 5&      -   5         ��Name$ tbx_log��5&       �� 
IsReadOnly . $ �� HorizontalScrollBarVisibility$ Visible=�6=    �� TextWrapping$
 Wrap=�6e   $	��480��6y   $	��220��6�   6�   5'      5(      5)      5*      
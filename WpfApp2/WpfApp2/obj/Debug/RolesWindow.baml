   M S B A M L   `   `   `  ����  WpfApp2#    UserManagementApp.RolesWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$'  !Управление ролями��5   	   $	��450��6#   $	��600��60    � WindowStartupLocation$ CenterOwner=�5   	    � 	Resources 5       �� 
TargetType �� Property �� Value��%j  '��       '�� j     '�� �     �� 5   
   " ��6   .�5      ��  � ��$ 5q�6'   �� 5	       � ��$ 5q�6(   5
      �� 5   
   " ��6   .�5      ��  � ��$ 5q�6(   �� 5       � ��$ 80��6)   5      �� 5   
   " ��6   .�5      ��  � ��$ 5,5,5,2q�6'   �� 5       � ��$ SemiBold"�6+   5      5      .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $'��!Управление ролями��6    $��0,0,0,10q�6;   .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      �� -   5         ��Name$ RoleNameTextBox��5      $	��200��63   $��5q�6?   �� 5      -   5        	 ��Name$	 AddRoleButton��5      $��Добавить��60   $��5q�6C   6N   �� 5      -   5        $	 UpdateRoleButton��5      $��Обновить��63   $��5q�6F   6Q   �� 5       -   5        $	 DeleteRoleButton��5       $��Удалить��63   $��5q�6E   6P   �� 5!      -   5        $	 
BackButton��5!      $��
Назад��6-   $��5q�6=   6H   5"      5#      &   System.Windows.Controls.DataGrid  5%   
   -   5        
  Name$
 RolesDataGrid��5%      $��1��6*   $��0q�67      AutoGenerateColumns .     
IsReadOnly .    SelectionMode$ Single=�5'   %      Columns 5(      0  *System.Windows.Controls.DataGridTextColumn  5)      ,  &System.Windows.Controls.DataGridColumn   Header$ ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6L   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *RoleID+6L     5*      $ Название роли��6%   $ * 6Z    6<   �� *	Role_name+6Z   5,      5-      5.      
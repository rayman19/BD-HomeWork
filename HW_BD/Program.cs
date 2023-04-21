using System;
using lcpi.data.oledb;

namespace HW_BD
{
	class Program
	{
		const string c_cn_str = "file name=bd.udl";
		const string cn_str = "provider=LCPI.IBProvider.5;" + "location=localhost:e:\\CREDITS.fdb;" + "user id=SYSDBA;\n" + "password=masterkey;\n" + "dbclient_library=fbclient.dll";
		const string c_tab = "\t\t\t\t\t";
		const string c_str = "\n\n\n\n\n\n\n\n";

		static void Select(OleDbConnection cn, string name_table)
		{
			string c_sql = "select * from " + name_table;

			using (var tr = cn.BeginTransaction())
			{
				using (var cmd = new OleDbCommand(c_sql, cn, tr))
				{
					using (var reader = cmd.ExecuteReader())
					{
						if (name_table == "CLIENTS")
						{
							Console.WriteLine("{0, 8}{1, 30}{2, 13}{3, 25}{4, 8}", "ID", "FIO", "PASSPORT", "JOB", "COST");
							while (reader.Read())
							{
								Console.Write("{0, 8}", reader.GetValue(0));
								Console.Write("{0, 30}", reader.GetValue(1));
								Console.Write("{0, 13}", reader.GetValue(2));
								Console.Write("{0, 25}", reader.GetValue(3));
								Console.Write("{0, 8}", reader.GetValue(4));
								Console.WriteLine("\n");
							}
						}
						else if (name_table == "CREDITS")
						{
							Console.WriteLine("{0, 8}{1, 18}{2, 30}{3, 12}{4, 8}{5, 8}{6, 20}", "ID", "COST", "REPAYMENT", "ID_CLIENT", "RATE", "ISSUE", "PURPOSE");
							while (reader.Read())
							{
								Console.Write("{0, 8}", reader.GetValue(0));
								Console.Write("{0, 18}", reader.GetValue(1));
								Console.Write("{0, 30}", reader.GetValue(2));
								Console.Write("{0, 12}", reader.GetValue(3));
								Console.Write("{0, 8}", reader.GetValue(4));
								Console.Write("{0, 8}", reader.GetValue(5));
								Console.Write("{0, 20}", reader.GetValue(6));
								Console.WriteLine("\n");
							}
						}
						else if (name_table == "PAYMENT")
						{
							Console.WriteLine("{0, 8}{1, 30}{2, 10}{3, 12}{4, 12}", "ID", "DATE PAYMENT", "COST", "ID_CREDIT", "ID_PAYER");
							while (reader.Read())
							{
								Console.Write("{0, 8}", reader.GetValue(0));
								Console.Write("{0, 30}", reader.GetValue(1));
								Console.Write("{0, 10}", reader.GetValue(2));
								Console.Write("{0, 12}", reader.GetValue(3));
								Console.Write("{0, 12}", reader.GetValue(4));
								Console.WriteLine("\n");
							}
						}
					}
				}
				tr.Commit();
			}

			Console.ReadKey();
		}
		static void Delete(OleDbConnection cn, string name_table)
		{
			Console.Clear();
			Console.WriteLine(c_tab + "УДАЛЕНИЕ ЗАПИСИ ИЗ " + name_table);
			Select(cn, name_table);
			Console.Write(c_tab + "Введите номер id по которому запись будет удалена: ");
			string id = Console.ReadLine();
			string c_sql = "DELETE FROM " + name_table + " WHERE id = " + id;

			using (var tr = cn.BeginTransaction())
			{
				using (var cmd = new OleDbCommand(c_sql, cn, tr))
				{
					cmd.ExecuteNonQuery();
				}
				tr.Commit();
			}

			Console.Write("\n\n" + c_tab + "Запись была удалена!");
			Console.ReadKey();
		}
		static void Insert(OleDbConnection cn, string name_table)
		{
			string c_sql;
			Console.Clear();
			Console.WriteLine(c_tab + "\tДОБАВЛЕНИЕ ЗАПИСИ В " + name_table);
			if (name_table == "CLIENTS")
			{
				Console.Write("\tВведите FIO: ");
				string fio = "'" + Console.ReadLine() + "',";
				Console.Write("\tВведите PASSPORT: ");
				string passport = "'" + Console.ReadLine() + "',";
				Console.Write("\tВведите JOB: ");
				string job = "'" + Console.ReadLine() + "',";
				Console.Write("\tВведите INCOME: ");
				string income = Console.ReadLine() + ")";
				c_sql = "INSERT INTO " + name_table + " (FIO, PASSPORT, JOB, INCOME) "
					+ "VALUES(" + fio + passport + job + income;
			}
			else if (name_table == "CREDITS")
			{
				Console.Write("\tВведите COST: ");
				string cost = Console.ReadLine() + ",";
				Console.Write("\tВведите REPAYMENT: ");
				string repayment = "'" + Console.ReadLine() + "',";
				Console.Write("\tВведите ID_CLIENT: ");
				string id_client = Console.ReadLine() + ",";
				Console.Write("\tВведите RATE: ");
				string rate = Console.ReadLine() + ",";
				Console.Write("\tВведите ISSUE: ");
				string issue = Console.ReadLine() + ",";
				Console.Write("\tВведите PURPOSE: ");
				string purpose = "'" + Console.ReadLine() + "')";
				c_sql = "INSERT INTO " + name_table + " (COST, REPAYMENT, ID_CLIENT, RATE, ISSUE, PURPOSE) "
					+ "VALUES(" + cost + repayment + id_client + rate + issue + purpose;
			}
			else if (name_table == "PAYMENT")
			{
				Console.Write("\tВведите COST: ");
				string cost = Console.ReadLine() + ",";
				Console.Write("\tВведите ID_CREDIT: ");
				string id_credit = Console.ReadLine() + ",";
				Console.Write("\tВведите ID_PAYER: ");
				string id_payer = Console.ReadLine() + ")";
				c_sql = "INSERT INTO " + name_table + " (DATE_PAY, COST, ID_CREDIT, ID_PAYER) "
					+ "VALUES(current_date," + cost + id_credit + id_payer;
			}
			else return;

			using (var tr = cn.BeginTransaction())
			{
				using (var cmd = new OleDbCommand(c_sql, cn, tr))
				{
					cmd.ExecuteNonQuery();
				}
				tr.Commit();
			}

			Console.Write(c_tab + "Запись была добавлена!");
			Console.ReadKey();
		}
		static void Update(OleDbConnection cn, string name_table)
		{
			string c_sql;
			Console.Clear();
			Console.WriteLine(c_tab + "\tИЗМЕНЕНИЕ ЗАПИСИ В " + name_table);
			Select(cn, name_table);
			Console.Write(c_tab + "Введите номер id по которому запись будет изменена: ");
			string id = Console.ReadLine();

			if (name_table == "CLIENTS")
			{
				bool check = true;
				c_sql = "UPDATE " + name_table + " SET ";
				
				Console.Write("\tИзменить FIO? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите FIO: ");
					if (check)
					{
						check = false;
						c_sql += "fio = '" + Console.ReadLine() + "'";
					}
					else
						c_sql += ", fio = '" + Console.ReadLine() + "'";
				}
				
				Console.Write("\tИзменить PASSPORT? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите PASSPORT: ");
					if (check)
					{
						check = false;
						c_sql += "passport = '" + Console.ReadLine() + "'";
					}
					else
						c_sql += ", passport = '" + Console.ReadLine() + "'";
				}

				Console.Write("\tИзменить JOB? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите JOB: ");
					if (check)
					{
						check = false;
						c_sql += "job = '" + Console.ReadLine() + "'";
					}
					else
						c_sql += ", job = '" + Console.ReadLine() + "'";
				}

				Console.Write("\tИзменить INCOME? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите INCOME: ");
					if (check)
					{
						check = false;
						c_sql += "income = " + Console.ReadLine();
					}
					else
						c_sql += ", income = " + Console.ReadLine();
				}

				c_sql += " WHERE id = " + id;
			}
			else if (name_table == "CREDITS")
			{
				bool check = true;
				c_sql = "UPDATE " + name_table + " SET ";

				Console.Write("\tИзменить COST? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите COST: ");
					if (check)
					{
						check = false;
						c_sql += "cost = " + Console.ReadLine();
					}
					else
						c_sql += ", cost = " + Console.ReadLine();
				}

				Console.Write("\tИзменить REPAYMENT? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите REPAYMENT: ");
					if (check)
					{
						check = false;
						c_sql += "rapayment = " + Console.ReadLine();
					}
					else
						c_sql += ", rapayment = " + Console.ReadLine();
				}

				Console.Write("\tИзменить ID_CLIENT? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите ID_CLIENT: ");
					if (check)
					{
						check = false;
						c_sql += "id_client = " + Console.ReadLine();
					}
					else
						c_sql += ", id_client = " + Console.ReadLine();
				}

				Console.Write("\tИзменить RATE? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите RATE: ");
					if (check)
					{
						check = false;
						c_sql += "rate = " + Console.ReadLine();
					}
					else
						c_sql += ", rate = " + Console.ReadLine();
				}

				Console.Write("\tИзменить ISSUE? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите ISSUE: ");
					if (check)
					{
						check = false;
						c_sql += "issue = " + Console.ReadLine();
					}
					else
						c_sql += ", issue = " + Console.ReadLine();
				}

				Console.Write("\tИзменить PURPOSE? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите PURPOSE: ");
					if (check)
					{
						check = false;
						c_sql += "purpose = '" + Console.ReadLine() + "'";
					}
					else
						c_sql += ", purpose = '" + Console.ReadLine() + "'";
				}

				c_sql += " WHERE id = " + id;
			}
			else if (name_table == "PAYMENT")
			{
				bool check = true;
				c_sql = "UPDATE " + name_table + " SET ";

				Console.Write("\tИзменить COST? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите COST: ");
					if (check)
					{
						check = false;
						c_sql += "cost = " + Console.ReadLine();
					}
					else
						c_sql += ", cost = " + Console.ReadLine();
				}

				Console.Write("\tИзменить ID_CREDIT? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите ID_CREDIT: ");
					if (check)
					{
						check = false;
						c_sql += "id_credit = " + Console.ReadLine();
					}
					else
						c_sql += ", id_credit = " + Console.ReadLine();
				}

				Console.Write("\tИзменить ID_PAYER? (y/..): ");
				if (Convert.ToString(Console.ReadLine()) == "y")
				{
					Console.Write("\t\tВведите ID_PAYER: ");
					if (check)
					{
						check = false;
						c_sql += "id_payer = " + Console.ReadLine();
					}
					else
						c_sql += ", id_payer = " + Console.ReadLine();
				}

				c_sql += " WHERE id = " + id;
			}
			else return;

			using (var tr = cn.BeginTransaction())
			{
				using (var cmd = new OleDbCommand(c_sql, cn, tr))
				{
					cmd.ExecuteNonQuery();
				}
				tr.Commit();
			}

			Console.Write(c_tab + "Запись была изменена!");
			Console.ReadKey();
		}
		static string GetNameTable()
		{
			Console.Clear();
			Console.WriteLine(c_str);
			Console.WriteLine(c_tab + "\tДОСТУПНЫЕ ТАБЛИЦЫ");
			Console.WriteLine(c_tab + "1. CLIENTS");
			Console.WriteLine(c_tab + "2. CREDITS");
			Console.WriteLine(c_tab + "3. PAYMENT");
			Console.Write(c_tab + "\tВведите номер таблицы: ");
			int key = Convert.ToInt32(Console.ReadLine());
			switch (key)
			{
				case (1): return "CLIENTS";
				case (2): return "CREDITS";
				case (3): return "PAYMENT";
			}
			return "None";
		}
		static void Main(string[] args)
		{
			bool isTransaktionActive = false;
			int key;
			string name_table;
			var cn = new OleDbConnection("file name=test.udl");
			cn.Open();
			do
			{
				Console.Clear();
				Console.WriteLine(c_str);
				Console.WriteLine(c_tab + "\tБАЗА ДАННЫХ КРЕДИТОВ");
				if (!isTransaktionActive)
				{
					Console.WriteLine(c_tab + "1. Запуск транзакции.");
					Console.WriteLine(c_tab + "2. Настройка транзакции.");
					Console.WriteLine(c_tab + "0. Выход из приложения.");
					Console.Write(c_tab + "\tВведите пункт: ");
					key = Convert.ToInt32(Console.ReadLine());
					if (key == 1)
					{
						isTransaktionActive = true;
					}
				}
				else
				{
					Console.WriteLine(c_tab + "1. Просмотр записей таблиц.");
					Console.WriteLine(c_tab + "2. Добавление записи в таблицу.");
					Console.WriteLine(c_tab + "3. Удаление записи из таблиц.");
					Console.WriteLine(c_tab + "4. Изменение записи в таблице.");
					Console.WriteLine(c_tab + "0. Выход из приложения.");
					Console.Write(c_tab + "\tВведите пункт: ");
					key = Convert.ToInt32(Console.ReadLine());
					if (key == 1)
					{
						name_table = GetNameTable();
						Console.Clear();
						Console.WriteLine(c_tab + "\tПРОСМОТР ТАБЛИЦЫ " + name_table);
						if (name_table == "None")
						{
							Console.Write(c_tab + "Введенный номер таблицы отсутсвует!");
							Console.ReadKey();
						}
						else
							Select(cn, name_table);
					}
					if (key == 2)
					{
						name_table = GetNameTable();
						if (name_table == "None")
						{
							Console.Write(c_tab + "Введенный номер таблицы отсутсвует!");
							Console.ReadKey();
						}
						else
							Insert(cn, name_table);
					}
					if (key == 3)
					{
						name_table = GetNameTable();
						if (name_table == "None")
						{
							Console.Write(c_tab + "Введенный номер таблицы отсутсвует!");
							Console.ReadKey();
						}
						else
							Delete(cn, name_table);
					}
					if (key == 4)
					{
						name_table = GetNameTable();
						if (name_table == "None")
						{
							Console.Write(c_tab + "Введенный номер таблицы отсутсвует!");
							Console.ReadKey();
						}
						else
							Update(cn, name_table);
					}
				}			
				if ((isTransaktionActive) && (key == 0))
					isTransaktionActive = false;
				else if (key == 0)
					break;
			} while (true);
			cn.Close();
		}
	}
}
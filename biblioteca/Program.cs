using System;
using System.Collections.Generic;

namespace SistemaISP
{
    // ==========================================
    // 📄 MODELO BASE (Compartido por el grupo)
    // ==========================================
    public class Cliente
    {
        public int IdCliente { get; set; }
        public string Nombre { get; set; }
        public int PlanMegas { get; set; }     // Velocidad (ej: 100, 300, 500 Mbps)
        public bool EstadoPago { get; set; }    // true = Pagado, false = Deudor
        public double Deuda { get; set; }       // Monto del recibo
    }

    // ==========================================
    // 👤 BIBLIOTECA - INTEGRANTE 1 (Modificado con Catálogo de Planes)
    // ==========================================
    public class AltaCliente
    {
        // Recibe como parámetro la lista global para añadir un cliente
        public void RegistrarCliente(List<Cliente> listaGlobal)
        {
            Console.Clear();
            Console.WriteLine("=== REGISTRAR ALTA DE CLIENTE ===");
            Cliente nuevo = new Cliente();

            Console.Write("Ingrese DNI o carnet extranjeria: ");
            nuevo.IdCliente = int.Parse(Console.ReadLine());

            Console.Write("Ingrese nombre completo: ");
            nuevo.Nombre = Console.ReadLine();

            // --- NUEVO SELECCIONADOR DE PLANES ---
            int seleccionPlan = 0;
            do
            {
                Console.WriteLine("\n--- SELECCIONE UN PLAN DE INTERNET ---");
                Console.WriteLine("1. Plan Hogar Básico   : 150 Mbps -> S/. 59.90 al mes");
                Console.WriteLine("2. Plan Fibra Avanzado : 300 Mbps -> S/. 89.90 al mes");
                Console.WriteLine("3. Plan Gaming Ultra   : 600 Mbps -> S/. 119.90 al mes");
                Console.Write("Elija una opción (1-3): ");

                if (!int.TryParse(Console.ReadLine(), out seleccionPlan) || seleccionPlan < 1 || seleccionPlan > 3)
                {
                    Console.WriteLine("[Error] Opción no válida. Por favor, elija 1, 2 o 3.");
                    seleccionPlan = 0;
                }
            } while (seleccionPlan == 0);

            // Asignación automática de Megas y Monto según lo elegido
            switch (seleccionPlan)
            {
                case 1:
                    nuevo.PlanMegas = 150;
                    nuevo.Deuda = 59.90;
                    break;
                case 2:
                    nuevo.PlanMegas = 300;
                    nuevo.Deuda = 89.90;
                    break;
                case 3:
                    nuevo.PlanMegas = 600;
                    nuevo.Deuda = 119.90;
                    break;
            }

            // Al ser nuevo, inicia con el mes pendiente de pago
            nuevo.EstadoPago = false;

            listaGlobal.Add(nuevo);
            Console.WriteLine($"\n[✓] {nuevo.Nombre} registrado con éxito en el Plan de {nuevo.PlanMegas} Mbps.");
        }
    }

    // ==========================================
    // 👤 BIBLIOTECA - INTEGRANTE 2 (Visualización)
    // ==========================================
    public class VisorClientes
    {
        public void MostrarTodosLosClientes(List<Cliente> listaGlobal)
        {
            Console.Clear();
            Console.WriteLine("=== LISTADO GENERAL DE CLIENTES ISP ===");
            Console.WriteLine("-----------------------------------------------------------------------------");
            Console.WriteLine(string.Format("{0,-10} | {1,-22} | {2,-12} | {3,-12} | {4,-10}", "ID", "Nombre", "Plan (Megas)", "Estado Pago", "Deuda"));
            Console.WriteLine("-----------------------------------------------------------------------------");

            if (listaGlobal.Count == 0)
            {
                Console.WriteLine("No hay clientes registrados en el sistema.");
                return;
            }

            foreach (var cli in listaGlobal)
            {
                string status = cli.EstadoPago ? "Al Día" : "Deudor";
                Console.WriteLine(string.Format("{0,-10} | {1,-22} | {2,-12} | {3,-12} | S/. {4,-8:F2}",
                    cli.IdCliente, cli.Nombre, cli.PlanMegas + " Mbps", status, cli.Deuda));
            }
            Console.WriteLine("-----------------------------------------------------------------------------");
        }
    }

    // ==========================================
    // 👤 BIBLIOTECA - INTEGRANTE 3 (Facturación)
    // ==========================================
    public class Facturacion
    {
        public void RegistrarPago(List<Cliente> listaGlobal, int idBuscar)
        {
            Console.Clear();
            Console.WriteLine("=== MÓDULO DE CAJA Y FACTURACIÓN ===");

            Cliente clienteEncontrado = listaGlobal.Find(c => c.IdCliente == idBuscar);

            if (clienteEncontrado != null)
            {
                if (clienteEncontrado.EstadoPago == true)
                {
                    Console.WriteLine($"El cliente {clienteEncontrado.Nombre} ya se encuentra al día en sus pagos.");
                }
                else
                {
                    Console.WriteLine($"Cliente Encontrado: {clienteEncontrado.Nombre}");
                    Console.WriteLine($"Monto a pagar: S/. {clienteEncontrado.Deuda:F2}");
                    Console.Write("¿Confirmar recepción del pago? (S/N): ");
                    string confirmar = Console.ReadLine().ToUpper();

                    if (confirmar == "S")
                    {
                        clienteEncontrado.EstadoPago = true;
                        clienteEncontrado.Deuda = 0.0;
                        Console.WriteLine("\n[✓] Pago procesado. Recibo emitido correctamente.");
                    }
                }
            }
            else
            {
                Console.WriteLine("[X] Error: El código de cliente no existe en la base de datos.");
            }
        }
    }

    // ==========================================
    // 👤 BIBLIOTECA - INTEGRANTE 4 (Corte de Servicio)
    // ==========================================
    public class CorteServicio
    {
        public void ListarClientesParaCorte(List<Cliente> listaGlobal)
        {
            Console.Clear();
            Console.WriteLine("=== ORDEN DE CORTE DE SERVICIO (CLIENTES DEUDORES) ===");

            List<Cliente> morosos = listaGlobal.FindAll(c => c.EstadoPago == false);

            if (morosos.Count == 0)
            {
                Console.WriteLine("¡Excelente! No hay clientes con recibos pendientes de pago.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Los siguientes usuarios presentan deuda activa y requieren corte técnico:\n");
                foreach (var cli in morosos)
                {
                    Console.WriteLine($"[⚠️ CORTAR] ID: {cli.IdCliente} | Cliente: {cli.Nombre} | Plan: {cli.PlanMegas}Mbps | Deuda: S/. {cli.Deuda:F2}");
                }
                Console.ResetColor();
            }
        }
    }

    // ==========================================
    // 💻 ARCHIVO CENTRAL (El Main Principal)
    // ==========================================
    class Program
    {
        static List<Cliente> baseDatosClientes = new List<Cliente>();

        static void Main(string[] args)
        {
            AltaCliente moduloAlta = new AltaCliente();
            VisorClientes moduloVisor = new VisorClientes();
            Facturacion moduloFacturas = new Facturacion();
            CorteServicio moduloCortes = new CorteServicio();

            // Carga inicial de usuarios con los planes estándar asignados
            baseDatosClientes.Add(new Cliente { IdCliente = 2001, Nombre = "Juan Pérez", PlanMegas = 300, EstadoPago = true, Deuda = 0.0 });
            baseDatosClientes.Add(new Cliente { IdCliente = 2002, Nombre = "María Rojas", PlanMegas = 500, EstadoPago = false, Deuda = 89.90 });
            baseDatosClientes.Add(new Cliente { IdCliente = 2003, Nombre = "Richard Celestino", PlanMegas = 300, EstadoPago = true, Deuda = 0.0 });
            baseDatosClientes.Add(new Cliente { IdCliente = 2004, Nombre = "Adrian Mendizabal", PlanMegas = 300, EstadoPago = false, Deuda = 89.90 });
            baseDatosClientes.Add(new Cliente { IdCliente = 2005, Nombre = "Andy Mendoza", PlanMegas = 150, EstadoPago = true, Deuda = 0.0 });
            baseDatosClientes.Add(new Cliente { IdCliente = 2006, Nombre = "Honorario Perez", PlanMegas = 600, EstadoPago = false, Deuda = 119.90 });
            baseDatosClientes.Add(new Cliente { IdCliente = 2007, Nombre = "Alvaro Timoteo", PlanMegas = 150, EstadoPago = false, Deuda = 59.90 });

            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("   SISTEMA DE GESTIÓN ISP - INTERNET    ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Alta de Cliente Nuevo");
                Console.WriteLine("2. Visualizar Listado de Clientes");
                Console.WriteLine("3. Registrar Pago de Recibo (Caja)");
                Console.WriteLine("4. Reporte de Cortes por Morosidad");
                Console.WriteLine("5. Salir");
                Console.Write("\nSeleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    opcion = 0;
                }

                switch (opcion)
                {
                    case 1:
                        moduloAlta.RegistrarCliente(baseDatosClientes);
                        break;

                    case 2:
                        moduloVisor.MostrarTodosLosClientes(baseDatosClientes);
                        break;

                    case 3:
                        Console.Write("\nIngrese el ID del cliente a pagar: ");
                        if (int.TryParse(Console.ReadLine(), out int idBuscar))
                        {
                            moduloFacturas.RegistrarPago(baseDatosClientes, idBuscar);
                        }
                        else
                        {
                            Console.WriteLine("ID no válido.");
                        }
                        break;

                    case 4:
                        moduloCortes.ListarClientesParaCorte(baseDatosClientes);
                        break;

                    case 5:
                        Console.WriteLine("\nCerrando el sistema del proveedor de internet. ¡Buen día!");
                        break;

                    default:
                        Console.WriteLine("\n[Error] Opción inválida. Intente de nuevo.");
                        break;
                }

                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();

            } while (opcion != 5);
        }
    }
}

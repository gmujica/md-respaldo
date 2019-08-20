namespace MesaDinero.Data.PersistenceModel
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MesaDineroContext : DbContext
    {
        public MesaDineroContext()
            : base("name=MesaDineroContext")
        {
        }

        public virtual DbSet<Tb_MD_Accionistas> Tb_MD_Accionistas { get; set; }
        public virtual DbSet<Tb_MD_ActividadEconomica> Tb_MD_ActividadEconomica { get; set; }
        public virtual DbSet<Tb_MD_Cargo> Tb_MD_Cargo { get; set; }
        public virtual DbSet<Tb_MD_Clientes> Tb_MD_Clientes { get; set; }
        public virtual DbSet<Tb_MD_ClientesDatosBancos> Tb_MD_ClientesDatosBancos { get; set; }
        public virtual DbSet<Tb_MD_ClienteUsuario> Tb_MD_ClienteUsuario { get; set; }
        public virtual DbSet<Tb_MD_Constantes> Tb_MD_Constantes { get; set; }
        public virtual DbSet<Tb_MD_Cuentas_Bancarias> Tb_MD_Cuentas_Bancarias { get; set; }
        public virtual DbSet<Tb_MD_Cuentas_Email> Tb_MD_Cuentas_Email { get; set; }
        public virtual DbSet<Tb_MD_Departamento> Tb_MD_Departamento { get; set; }
        public virtual DbSet<Tb_MD_Distrito> Tb_MD_Distrito { get; set; }
        public virtual DbSet<Tb_MD_DocOrdenPagoSubasta> Tb_MD_DocOrdenPagoSubasta { get; set; }
        public virtual DbSet<Tb_MD_Documentos> Tb_MD_Documentos { get; set; }
        public virtual DbSet<Tb_MD_Empresa_PersonaAutorizada> Tb_MD_Empresa_PersonaAutorizada { get; set; }
        public virtual DbSet<Tb_MD_Entidades_Financieras> Tb_MD_Entidades_Financieras { get; set; }
        public virtual DbSet<Tb_MD_Expuesto_Politicamente> Tb_MD_Expuesto_Politicamente { get; set; }
        public virtual DbSet<Tb_MD_Mae_Usuarios> Tb_MD_Mae_Usuarios { get; set; }
        public virtual DbSet<Tb_MD_Notificacion> Tb_MD_Notificacion { get; set; }
        public virtual DbSet<Tb_MD_Observacion_Cliente> Tb_MD_Observacion_Cliente { get; set; }
        public virtual DbSet<Tb_MD_OrigenFondo> Tb_MD_OrigenFondo { get; set; }
        public virtual DbSet<Tb_MD_Pagina> Tb_MD_Pagina { get; set; }
        public virtual DbSet<Tb_MD_Pais> Tb_MD_Pais { get; set; }
        public virtual DbSet<Tb_MD_PefilPagina> Tb_MD_PefilPagina { get; set; }
        public virtual DbSet<Tb_MD_Per_Juridica> Tb_MD_Per_Juridica { get; set; }
        public virtual DbSet<Tb_MD_Per_Natural> Tb_MD_Per_Natural { get; set; }
        public virtual DbSet<Tb_MD_Perfiles> Tb_MD_Perfiles { get; set; }
        public virtual DbSet<Tb_MD_PerfilUsuario> Tb_MD_PerfilUsuario { get; set; }
        public virtual DbSet<Tb_MD_Pre_Accionistas> Tb_MD_Pre_Accionistas { get; set; }
        public virtual DbSet<Tb_MD_Pre_Clientes> Tb_MD_Pre_Clientes { get; set; }
        public virtual DbSet<Tb_MD_Pre_ClientesDatosBancos> Tb_MD_Pre_ClientesDatosBancos { get; set; }
        public virtual DbSet<Tb_MD_Pre_Empresa_PersonaAutorizada> Tb_MD_Pre_Empresa_PersonaAutorizada { get; set; }
        public virtual DbSet<Tb_MD_Pre_Per_Juridica> Tb_MD_Pre_Per_Juridica { get; set; }
        public virtual DbSet<Tb_MD_Pre_Per_Natural> Tb_MD_Pre_Per_Natural { get; set; }
        public virtual DbSet<Tb_MD_Proceso_Subasta> Tb_MD_Proceso_Subasta { get; set; }
        public virtual DbSet<Tb_MD_Provincia> Tb_MD_Provincia { get; set; }
        public virtual DbSet<Tb_MD_RecuperarPassword> Tb_MD_RecuperarPassword { get; set; }
        public virtual DbSet<Tb_MD_Subasta> Tb_MD_Subasta { get; set; }
        public virtual DbSet<Tb_MD_Subasta_Detalle> Tb_MD_Subasta_Detalle { get; set; }
        public virtual DbSet<Tb_MD_Subasta_Pago> Tb_MD_Subasta_Pago { get; set; }
        public virtual DbSet<Tb_MD_Tiempos> Tb_MD_Tiempos { get; set; }
        public virtual DbSet<Tb_MD_Tipo_Cambio_Garantizado> Tb_MD_Tipo_Cambio_Garantizado { get; set; }
        public virtual DbSet<Tb_MD_Tipo_Cambio_Mercado> Tb_MD_Tipo_Cambio_Mercado { get; set; }
        public virtual DbSet<Tb_MD_TipoCuentaBancaria> Tb_MD_TipoCuentaBancaria { get; set; }
        public virtual DbSet<Tb_MD_TipoDocumento> Tb_MD_TipoDocumento { get; set; }
        public virtual DbSet<Tb_MD_TipoMoneda> Tb_MD_TipoMoneda { get; set; }
        public virtual DbSet<Tb_MD_Transaccion> Tb_MD_Transaccion { get; set; }
        public virtual DbSet<Tb_MD_Ubigeo> Tb_MD_Ubigeo { get; set; }
        public virtual DbSet<Tb_RegistroPersonaAutorizada> Tb_RegistroPersonaAutorizada { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tb_MD_Accionistas>()
                .Property(e => e.IdPersonaNatural)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Accionistas>()
                .Property(e => e.IdEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ActividadEconomica>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ActividadEconomica>()
                .HasMany(e => e.Tb_MD_Per_Juridica)
                .WithOptional(e => e.Tb_MD_ActividadEconomica)
                .HasForeignKey(e => e.ActividadEconomica);

            modelBuilder.Entity<Tb_MD_ActividadEconomica>()
                .HasMany(e => e.Tb_MD_Pre_Per_Juridica)
                .WithOptional(e => e.Tb_MD_ActividadEconomica)
                .HasForeignKey(e => e.ActividadEconomica);

            modelBuilder.Entity<Tb_MD_ActividadEconomica>()
                .HasMany(e => e.Tb_MD_Pre_Per_Juridica1)
                .WithOptional(e => e.Tb_MD_ActividadEconomica1)
                .HasForeignKey(e => e.ActividadEconomica);

            modelBuilder.Entity<Tb_MD_Cargo>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Cargo>()
                .HasMany(e => e.Tb_MD_Empresa_PersonaAutorizada)
                .WithRequired(e => e.Tb_MD_Cargo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Cargo>()
                .HasMany(e => e.Tb_MD_Pre_Empresa_PersonaAutorizada)
                .WithRequired(e => e.Tb_MD_Cargo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Clientes>()
                .Property(e => e.vNombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Clientes>()
                .Property(e => e.vApellido)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Clientes>()
                .Property(e => e.vEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Clientes>()
                .Property(e => e.vCelular)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Clientes>()
                .Property(e => e.vTipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Clientes>()
                .Property(e => e.vNroDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Clientes>()
                .Property(e => e.NombreCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Clientes>()
                .HasMany(e => e.Tb_MD_ClienteUsuario)
                .WithRequired(e => e.Tb_MD_Clientes)
                .HasForeignKey(e => e.IdCliente)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_ClientesDatosBancos>()
                .Property(e => e.vBanco)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ClientesDatosBancos>()
                .Property(e => e.vMoneda)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ClientesDatosBancos>()
                .Property(e => e.vNroCuenta)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ClientesDatosBancos>()
                .Property(e => e.vCCI)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ClientesDatosBancos>()
                .Property(e => e.vNroDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ClientesDatosBancos>()
                .Property(e => e.vRUCUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ClientesDatosBancos>()
                .Property(e => e.vRegPAutorizada)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ClienteUsuario>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ClienteUsuario>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ClienteUsuario>()
                .Property(e => e.NombreCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ClienteUsuario>()
                .Property(e => e.Iniciales)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_ClienteUsuario>()
                .Property(e => e.vNroDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Constantes>()
                .Property(e => e.IdConstante)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Constantes>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Constantes>()
                .Property(e => e.Valor)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Cuentas_Bancarias>()
                .Property(e => e.vNumDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Cuentas_Bancarias>()
                .Property(e => e.vCodEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Cuentas_Bancarias>()
                .Property(e => e.vNumCuenta)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Cuentas_Bancarias>()
                .Property(e => e.vNumCCI)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Cuentas_Bancarias>()
                .Property(e => e.vMonedaCuenta)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Cuentas_Email>()
                .Property(e => e.vNumDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Cuentas_Email>()
                .Property(e => e.vMailContacto)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Cuentas_Email>()
                .Property(e => e.iIdPerjuridica)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Cuentas_Email>()
                .Property(e => e.vRol)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Departamento>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Departamento>()
                .Property(e => e.idPais)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Departamento>()
                .HasMany(e => e.Tb_MD_Per_Juridica)
                .WithOptional(e => e.Tb_MD_Departamento)
                .HasForeignKey(e => e.iCodDepartamento);

            modelBuilder.Entity<Tb_MD_Departamento>()
                .HasMany(e => e.Tb_MD_Per_Natural)
                .WithOptional(e => e.Tb_MD_Departamento)
                .HasForeignKey(e => e.iCodDepartamento);

            modelBuilder.Entity<Tb_MD_Departamento>()
                .HasMany(e => e.Tb_MD_Pre_Per_Juridica)
                .WithOptional(e => e.Tb_MD_Departamento)
                .HasForeignKey(e => e.iCodDepartamento);

            modelBuilder.Entity<Tb_MD_Departamento>()
                .HasMany(e => e.Tb_MD_Pre_Per_Natural)
                .WithOptional(e => e.Tb_MD_Departamento)
                .HasForeignKey(e => e.iCodDepartamento);

            modelBuilder.Entity<Tb_MD_Departamento>()
                .HasMany(e => e.Tb_MD_Ubigeo)
                .WithRequired(e => e.Tb_MD_Departamento)
                .HasForeignKey(e => e.CodDepartamento)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Distrito>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Distrito>()
                .HasMany(e => e.Tb_MD_Per_Juridica)
                .WithOptional(e => e.Tb_MD_Distrito)
                .HasForeignKey(e => e.iCodDistrito);

            modelBuilder.Entity<Tb_MD_Distrito>()
                .HasMany(e => e.Tb_MD_Per_Natural)
                .WithOptional(e => e.Tb_MD_Distrito)
                .HasForeignKey(e => e.iCodDistrito);

            modelBuilder.Entity<Tb_MD_Distrito>()
                .HasMany(e => e.Tb_MD_Pre_Per_Juridica)
                .WithOptional(e => e.Tb_MD_Distrito)
                .HasForeignKey(e => e.iCodDistrito);

            modelBuilder.Entity<Tb_MD_Distrito>()
                .HasMany(e => e.Tb_MD_Pre_Per_Natural)
                .WithOptional(e => e.Tb_MD_Distrito)
                .HasForeignKey(e => e.iCodDistrito);

            modelBuilder.Entity<Tb_MD_Distrito>()
                .HasMany(e => e.Tb_MD_Ubigeo)
                .WithRequired(e => e.Tb_MD_Distrito)
                .HasForeignKey(e => e.CodDistrito)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Documentos>()
                .Property(e => e.vNombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Documentos>()
                .Property(e => e.vExtension)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Documentos>()
                .Property(e => e.vTipo)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Documentos>()
                .HasMany(e => e.Tb_MD_DocOrdenPagoSubasta)
                .WithRequired(e => e.Tb_MD_Documentos)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Empresa_PersonaAutorizada>()
                .Property(e => e.IdEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Empresa_PersonaAutorizada>()
                .Property(e => e.IdPersonaAutorizada)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .Property(e => e.vCodEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .Property(e => e.vDesEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .Property(e => e.vLogoEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .Property(e => e.VTipo)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .Property(e => e.vFormatoCCI)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .Property(e => e.vFormatoCuentaBancaria)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .HasMany(e => e.Tb_MD_ClientesDatosBancos)
                .WithOptional(e => e.Tb_MD_Entidades_Financieras)
                .HasForeignKey(e => e.vBanco);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .HasMany(e => e.Tb_MD_Pre_ClientesDatosBancos)
                .WithOptional(e => e.Tb_MD_Entidades_Financieras)
                .HasForeignKey(e => e.vBanco);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .HasMany(e => e.Tb_MD_Subasta_Pago)
                .WithOptional(e => e.Tb_MD_Entidades_Financieras)
                .HasForeignKey(e => e.vCodBancoCliente);

            modelBuilder.Entity<Tb_MD_Entidades_Financieras>()
                .HasMany(e => e.Tb_MD_Subasta_Pago1)
                .WithOptional(e => e.Tb_MD_Entidades_Financieras1)
                .HasForeignKey(e => e.vCodBancoDestinoCliente);

            modelBuilder.Entity<Tb_MD_Expuesto_Politicamente>()
                .Property(e => e.vNumDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Expuesto_Politicamente>()
                .Property(e => e.vRUCEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Expuesto_Politicamente>()
                .Property(e => e.vNombreEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Expuesto_Politicamente>()
                .Property(e => e.vCargo)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Expuesto_Politicamente>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Expuesto_Politicamente>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Expuesto_Politicamente>()
                .Property(e => e.iEstadoRegistro)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Expuesto_Politicamente>()
                .Property(e => e.vTipoPersona)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vTipoPersona)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vEmailUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vTipoUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vPassword)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vFlgValidado)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vUsuarioValidacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vCelular)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vTelefonoFijo)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vRucEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .Property(e => e.vNroDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Mae_Usuarios>()
                .HasMany(e => e.Tb_MD_PerfilUsuario)
                .WithRequired(e => e.Tb_MD_Mae_Usuarios)
                .HasForeignKey(e => e.IdUsuario)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Notificacion>()
                .Property(e => e.IdNotificacion);
            

            modelBuilder.Entity<Tb_MD_Notificacion>()
                .Property(e => e.IdUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Notificacion>()
                .Property(e => e.Titulo)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Notificacion>()
                .Property(e => e.Mensaje)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Observacion_Cliente>()
                .Property(e => e.Mensaje)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Observacion_Cliente>()
                .Property(e => e.Estado)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Observacion_Cliente>()
                .Property(e => e.RolObservador)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Observacion_Cliente>()
                .Property(e => e.TipoRegistroObservacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_OrigenFondo>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_OrigenFondo>()
                .HasMany(e => e.Tb_MD_Per_Juridica)
                .WithOptional(e => e.Tb_MD_OrigenFondo)
                .HasForeignKey(e => e.OrigenFondos);

            modelBuilder.Entity<Tb_MD_OrigenFondo>()
                .HasMany(e => e.Tb_MD_Per_Natural)
                .WithOptional(e => e.Tb_MD_OrigenFondo)
                .HasForeignKey(e => e.iOrigenFondos);

            modelBuilder.Entity<Tb_MD_OrigenFondo>()
                .HasMany(e => e.Tb_MD_Pre_Per_Juridica)
                .WithOptional(e => e.Tb_MD_OrigenFondo)
                .HasForeignKey(e => e.OrigenFondos);

            modelBuilder.Entity<Tb_MD_OrigenFondo>()
                .HasMany(e => e.Tb_MD_Pre_Per_Natural)
                .WithOptional(e => e.Tb_MD_OrigenFondo)
                .HasForeignKey(e => e.iOrigenFondos);

            modelBuilder.Entity<Tb_MD_Pagina>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pagina>()
                .Property(e => e.Ruta)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pagina>()
                .Property(e => e.Modulo)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pagina>()
                .Property(e => e.Icon)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pagina>()
                .HasMany(e => e.Tb_MD_PefilPagina)
                .WithRequired(e => e.Tb_MD_Pagina)
                .HasForeignKey(e => e.IdPagina)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Pais>()
                .Property(e => e.IdPais)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pais>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pais>()
                .HasMany(e => e.Tb_MD_Departamento)
                .WithRequired(e => e.Tb_MD_Pais)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Pais>()
                .HasMany(e => e.Tb_MD_Per_Juridica)
                .WithOptional(e => e.Tb_MD_Pais)
                .HasForeignKey(e => e.vIdPaisOrigen);

            modelBuilder.Entity<Tb_MD_Pais>()
                .HasMany(e => e.Tb_MD_Per_Natural)
                .WithOptional(e => e.Tb_MD_Pais)
                .HasForeignKey(e => e.vIdPaisOrigen);

            modelBuilder.Entity<Tb_MD_Pais>()
                .HasMany(e => e.Tb_MD_Pre_Per_Juridica)
                .WithOptional(e => e.Tb_MD_Pais)
                .HasForeignKey(e => e.vIdPaisOrigen);

            modelBuilder.Entity<Tb_MD_Pais>()
                .HasMany(e => e.Tb_MD_Pre_Per_Natural)
                .WithOptional(e => e.Tb_MD_Pais)
                .HasForeignKey(e => e.vIdPaisOrigen);

            modelBuilder.Entity<Tb_MD_Pais>()
                .HasMany(e => e.Tb_MD_Ubigeo)
                .WithRequired(e => e.Tb_MD_Pais)
                .HasForeignKey(e => e.CodPais)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vNumDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vRazonSocial)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vDireccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vUbigeoDireccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vCodigoPostal)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vTelefonoFijo)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vTelefonoMovil)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vRubro)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vMailEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vNumDocumentoCreaEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vNombreEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vRepresentanteLegal)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .Property(e => e.vIdPaisOrigen)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .HasMany(e => e.Tb_MD_Accionistas)
                .WithRequired(e => e.Tb_MD_Per_Juridica)
                .HasForeignKey(e => e.IdEmpresa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .HasMany(e => e.Tb_MD_Cuentas_Bancarias)
                .WithRequired(e => e.Tb_MD_Per_Juridica)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Per_Juridica>()
                .HasMany(e => e.Tb_MD_Empresa_PersonaAutorizada)
                .WithRequired(e => e.Tb_MD_Per_Juridica)
                .HasForeignKey(e => e.IdEmpresa)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vNumDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vUbigeoDireccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vDireccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vTelefonoMovil)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vOcupacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vMailContacto)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vIdPaisOrigen)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vFlgExpuestoPoliticamente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vTipoPersona)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vNombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vApellido)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vApellidoMat)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vTipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vPreCelular)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .Property(e => e.vFlgSituacionLaboral)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .HasMany(e => e.Tb_MD_Accionistas)
                .WithRequired(e => e.Tb_MD_Per_Natural)
                .HasForeignKey(e => e.IdPersonaNatural)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .HasMany(e => e.Tb_MD_Empresa_PersonaAutorizada)
                .WithRequired(e => e.Tb_MD_Per_Natural)
                .HasForeignKey(e => e.IdPersonaAutorizada)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Per_Natural>()
                .HasMany(e => e.Tb_MD_Per_Juridica)
                .WithOptional(e => e.Tb_MD_Per_Natural)
                .HasForeignKey(e => e.vRepresentanteLegal);

            modelBuilder.Entity<Tb_MD_Perfiles>()
                .Property(e => e.NombrePerfil)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Perfiles>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Perfiles>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Perfiles>()
                .Property(e => e.Modulo)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Perfiles>()
                .HasMany(e => e.Tb_MD_PefilPagina)
                .WithRequired(e => e.Tb_MD_Perfiles)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Perfiles>()
                .HasMany(e => e.Tb_MD_PerfilUsuario)
                .WithRequired(e => e.Tb_MD_Perfiles)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Pre_Accionistas>()
                .Property(e => e.IdPersonaNatural)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Accionistas>()
                .Property(e => e.vTipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Accionistas>()
                .Property(e => e.vNombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Accionistas>()
                .Property(e => e.vApellido)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Accionistas>()
                .Property(e => e.vApellidoMat)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Accionistas>()
                .Property(e => e.vTelefonoMovil)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Accionistas>()
                .Property(e => e.vPreCelular)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Accionistas>()
                .Property(e => e.vMailContacto)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Accionistas>()
                .Property(e => e.IdEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.vNroDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.vEmail)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.vClaveSMS)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.vNombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.vApellido)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.vCelular)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.Seguimiento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.vTipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.NombreCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.EstadoValidacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.EstadoValidacion_Fideicomiso)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.ComentarioOperador)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.ComentarioFideicomiso)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.vTipoRegistro)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.nroDocumentoContacto)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.nombreEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Clientes>()
                .Property(e => e.vTipoValidacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_ClientesDatosBancos>()
                .Property(e => e.vBanco)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_ClientesDatosBancos>()
                .Property(e => e.vMoneda)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_ClientesDatosBancos>()
                .Property(e => e.vNroCuenta)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_ClientesDatosBancos>()
                .Property(e => e.vCCI)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_ClientesDatosBancos>()
                .Property(e => e.vNroDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_ClientesDatosBancos>()
                .Property(e => e.vRUCUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_ClientesDatosBancos>()
                .Property(e => e.vRegPAutorizada)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Empresa_PersonaAutorizada>()
                .Property(e => e.IdEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Empresa_PersonaAutorizada>()
                .Property(e => e.IdPersonaAutorizada)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Empresa_PersonaAutorizada>()
                .Property(e => e.vTipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Empresa_PersonaAutorizada>()
                .Property(e => e.vNombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Empresa_PersonaAutorizada>()
                .Property(e => e.vApellido)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Empresa_PersonaAutorizada>()
                .Property(e => e.vApellidoMat)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Empresa_PersonaAutorizada>()
                .Property(e => e.vTelefonoMovil)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Empresa_PersonaAutorizada>()
                .Property(e => e.vPreCelular)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Empresa_PersonaAutorizada>()
                .Property(e => e.vMailContacto)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vNumDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vRazonSocial)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vDireccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vUbigeoDireccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vCodigoPostal)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vTelefonoFijo)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vTelefonoMovil)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vRubro)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vMailEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vNumDocumentoCreaEmpresa)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vNombreEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Juridica>()
                .Property(e => e.vIdPaisOrigen)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vNumDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vUbigeoDireccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vDireccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vTelefonoMovil)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vOcupacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vMailContacto)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vIdPaisOrigen)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vFlgExpuestoPoliticamente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vTipoPersona)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vNombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vApellido)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vApellidoMat)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vTipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vPreCelular)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.vFlgSituacionLaboral)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.NombreEntidadExpuesto)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .Property(e => e.CargoExpuesto)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Pre_Per_Natural>()
                .HasMany(e => e.Tb_MD_Pre_Per_Juridica)
                .WithOptional(e => e.Tb_MD_Pre_Per_Natural)
                .HasForeignKey(e => e.vRepresentanteLegal);

            modelBuilder.Entity<Tb_MD_Proceso_Subasta>()
                .Property(e => e.vIdProceso)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Proceso_Subasta>()
                .Property(e => e.vNombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Proceso_Subasta>()
                .Property(e => e.vSiguiente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Provincia>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Provincia>()
                .HasMany(e => e.Tb_MD_Per_Juridica)
                .WithOptional(e => e.Tb_MD_Provincia)
                .HasForeignKey(e => e.iCodProvincia);

            modelBuilder.Entity<Tb_MD_Provincia>()
                .HasMany(e => e.Tb_MD_Per_Natural)
                .WithOptional(e => e.Tb_MD_Provincia)
                .HasForeignKey(e => e.iCodProvincia);

            modelBuilder.Entity<Tb_MD_Provincia>()
                .HasMany(e => e.Tb_MD_Pre_Per_Juridica)
                .WithOptional(e => e.Tb_MD_Provincia)
                .HasForeignKey(e => e.iCodProvincia);

            modelBuilder.Entity<Tb_MD_Provincia>()
                .HasMany(e => e.Tb_MD_Pre_Per_Natural)
                .WithOptional(e => e.Tb_MD_Provincia)
                .HasForeignKey(e => e.iCodProvincia);

            modelBuilder.Entity<Tb_MD_Provincia>()
                .HasMany(e => e.Tb_MD_Ubigeo)
                .WithRequired(e => e.Tb_MD_Provincia)
                .HasForeignKey(e => e.CodProvincia)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_RecuperarPassword>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_RecuperarPassword>()
                .Property(e => e.TipoUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.vNumDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.vTipoOperacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.vEstadoSubasta)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.vNumInsPago)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.vNumDocPartner)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.vEstadoRegistro)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.vTipoPersona)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.vRUCUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.vRUCEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.vMonedaEnviaCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.vMonedaRecibeCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.nMontoEnviaCliente)
                .HasPrecision(21, 6);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.nMontiRecibeCliente)
                .HasPrecision(21, 6);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.NroOperacionPago)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .Property(e => e.NombreCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .HasMany(e => e.Tb_MD_DocOrdenPagoSubasta)
                .WithRequired(e => e.Tb_MD_Subasta)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .HasMany(e => e.Tb_MD_Subasta_Detalle)
                .WithRequired(e => e.Tb_MD_Subasta)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_Subasta>()
                .HasOptional(e => e.Tb_MD_Subasta_Pago)
                .WithRequired(e => e.Tb_MD_Subasta);

            modelBuilder.Entity<Tb_MD_Subasta_Detalle>()
                .Property(e => e.vTipoDetalle)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Detalle>()
                .Property(e => e.vNumDocPartner)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Detalle>()
                .Property(e => e.vTipoMoneda)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Detalle>()
                .Property(e => e.nValorCompra)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Tb_MD_Subasta_Detalle>()
                .Property(e => e.nValorVenta)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Tb_MD_Subasta_Detalle>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Detalle>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Detalle>()
                .Property(e => e.vEstadoRegistro)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Detalle>()
                .Property(e => e.vRUCEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Detalle>()
                .Property(e => e.RazonSocial)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Detalle>()
                .Property(e => e.TipoCambio)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vTipoPersona)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vNroDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vNumOperacionPago)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vCodBancoCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vNumeroCuenta)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vTipoMonedaTransferida)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.nMontoTransferido)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vCodBancoFideicomiso)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vNumeroCuentaFideicomiso)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vNumDocValidaDepositoOperaciones)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vEstadoValOperador)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vNumDocValidaDepositoFideicomiso)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vEstadoValFideicomiso)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vNumOpeBancoACliente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vCodBancoDestinoCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.nMontoTransferidoACliente)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vTipoMonedaDestinoCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vNumeroCuentaDestinoCliente)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vObservacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vNumeroLiquidacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vEstadoLiq)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vUsuarioLiqLmd)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vUsuarioPart)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vNroVoucherFid)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Subasta_Pago>()
                .Property(e => e.vUsuarioFid)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tiempos>()
                .Property(e => e.vCodTransaccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tiempos>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tiempos>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tiempos>()
                .Property(e => e.nTiempoTransFideicomiso)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tiempos>()
                .Property(e => e.vNroDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tiempos>()
                .Property(e => e.vRUCEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.vNumDocPartner)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.nRango1_n)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.nValorRangoMinimo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.nValorRangoMaximo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.vTipoMoneda)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.nValorCompra)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.nValorVenta)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.nPorComision)
                .HasPrecision(8, 6);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.nSpreed)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.vEstadoRegistro)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.vRUCEntidad)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Garantizado>()
                .Property(e => e.VRUCUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Mercado>()
                .Property(e => e.vCodBanco)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Mercado>()
                .Property(e => e.nRango1_n)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Mercado>()
                .Property(e => e.nValorRangoMinimo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Mercado>()
                .Property(e => e.nValorRangoMaximo)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Mercado>()
                .Property(e => e.vTipoMoneda)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Mercado>()
                .Property(e => e.nValorCompra)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Mercado>()
                .Property(e => e.nValorVenta)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Mercado>()
                .Property(e => e.nPorComision)
                .HasPrecision(8, 6);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Mercado>()
                .Property(e => e.nSpreed)
                .HasPrecision(18, 6);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Mercado>()
                .Property(e => e.VRUCUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Tipo_Cambio_Mercado>()
                .Property(e => e.vEstadoRegistro)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_TipoCuentaBancaria>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_TipoCuentaBancaria>()
                .HasMany(e => e.Tb_MD_ClientesDatosBancos)
                .WithOptional(e => e.Tb_MD_TipoCuentaBancaria)
                .HasForeignKey(e => e.iTipoCuenta);

            modelBuilder.Entity<Tb_MD_TipoCuentaBancaria>()
                .HasMany(e => e.Tb_MD_Cuentas_Bancarias)
                .WithRequired(e => e.Tb_MD_TipoCuentaBancaria)
                .HasForeignKey(e => e.iTipoCuenta)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tb_MD_TipoCuentaBancaria>()
                .HasMany(e => e.Tb_MD_Pre_ClientesDatosBancos)
                .WithOptional(e => e.Tb_MD_TipoCuentaBancaria)
                .HasForeignKey(e => e.iTipoCuenta);

            modelBuilder.Entity<Tb_MD_TipoDocumento>()
                .Property(e => e.IdTipoDocumento)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_TipoDocumento>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_TipoDocumento>()
                .Property(e => e.Tipo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_TipoMoneda>()
                .Property(e => e.vCodMoneda)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_TipoMoneda>()
                .Property(e => e.vDesMoneda)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_TipoMoneda>()
                .Property(e => e.vSimboloMoneda)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_TipoMoneda>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_TipoMoneda>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_TipoMoneda>()
                .Property(e => e.vRUCUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_TipoMoneda>()
                .HasMany(e => e.Tb_MD_ClientesDatosBancos)
                .WithOptional(e => e.Tb_MD_TipoMoneda)
                .HasForeignKey(e => e.vMoneda);

            modelBuilder.Entity<Tb_MD_TipoMoneda>()
                .HasMany(e => e.Tb_MD_Cuentas_Bancarias)
                .WithOptional(e => e.Tb_MD_TipoMoneda)
                .HasForeignKey(e => e.vMonedaCuenta);

            modelBuilder.Entity<Tb_MD_TipoMoneda>()
                .HasMany(e => e.Tb_MD_Pre_ClientesDatosBancos)
                .WithOptional(e => e.Tb_MD_TipoMoneda)
                .HasForeignKey(e => e.vMoneda);

            modelBuilder.Entity<Tb_MD_Transaccion>()
                .Property(e => e.vCodTransaccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Transaccion>()
                .Property(e => e.vNomTransaccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Transaccion>()
                .Property(e => e.vDesTransaccion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Transaccion>()
                .Property(e => e.vUsuarioCreacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Transaccion>()
                .Property(e => e.vUsuarioModificacion)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Transaccion>()
                .Property(e => e.vEstadoRegistro)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Transaccion>()
                .Property(e => e.vRUCUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Transaccion>()
                .Property(e => e.vIdUsuario)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Ubigeo>()
                .Property(e => e.CodPais)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_MD_Ubigeo>()
                .Property(e => e.CodUbigeo)
                .IsUnicode(false);

            modelBuilder.Entity<Tb_RegistroPersonaAutorizada>()
                .Property(e => e.vRegPAutorizada)
                .IsUnicode(false);
        }
    }
}

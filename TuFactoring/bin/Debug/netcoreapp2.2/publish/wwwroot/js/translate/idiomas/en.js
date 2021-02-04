﻿const idiomaEN = {
    $vuetify: {
        close: 'Close',
        dataIterator: {
            pageText: '{0}-{1} of {2}',
            noResultsText: 'No matching records found',
            loadingText: 'Loading items...',
        },
        dataTable: {
            itemsPerPageText: 'Rows per page:',
            ariaLabel: {
                sortDescending: ': Sorted descending. Activate to remove sorting.',
                sortAscending: ': Sorted ascending. Activate to sort descending.',
                sortNone: ': Not sorted. Activate to sort ascending.',
            },
            sortBy: 'Sort by',
        },
        dataFooter: {
            pageText: '{0}-{1} of {2}',
            itemsPerPageText: 'Items per page:',
            itemsPerPageAll: 'All',
            nextPage: 'Next page',
            prevPage: 'Previous page',
            firstPage: 'First page',
            lastPage: 'Last page',
        },
        datePicker: {
            itemsSelected: '{0} selected',
        },
        noDataText: 'No data available',
        carousel: {
            prev: 'Previous visual',
            next: 'Next visual',
        },
        calendar: {
            moreEvents: '{0} more',
        }
    },
    headers: {
        details: "Details",
        concept: "Concept",
        event: "Event",
        category: "Category",
        account: "Account",
        participant: "Participant",
        numeroFactura: "Invoice Number",
        accountNumber: "Account Number",
        numberRef: "Reference Number",
        numeroFechaVencimiento: "Invoice Number / Expiration Date",
        fechaVencimientoNumero: "Expiration Date / Invoice Number",
        discount: "Discount",
        commissionService: "Service Commission",
        commissionDiscount: "Discount / Service Commission",
        requestFinancing: "Financing",
        aceptado: "Accepted",
        doct: "Document",
        referenceNumber: "Reference Number",
        amountOfThePay: "Payment amount",
        receiptDate: "Receipt Date",
        paymentDate: "Payment Date",
        amountPayment: "Payment Amount",
        bank: "Bank",
        payReceivedBy: "Paid to",
        receiptBank: "Receiving Bank",
        paid: "Paid",
        pay: "Pay",
        payer: "Payer",
        commission: "Commission",
        discount: "Discount",
        financing: "Financing",
        cuentaReceptora: "Receiving Account",
        amountPaid: "Amount Paid",
        dataPaid: "Transaction Date",
        abbreviation: "Abbreviation",
        description: "Description",
        content: "Content",
        typeContent: "Content Type",
        detail: "Details",
        amountToPay: "Amount payable",
        receiver: "Pay to",
        payer:"",
        bankConfirming:"Confirming bank",
        n: "N°",
        check: "/",
        montoDescontarAnualizado: "Amount to Discount / Annualized Cost",
        ofertaPropuesta: "Proposal Offer",
        clienteComprador: "Client<br><small>buyer</small>",
        clienteBanco: "Client<br><small>Bank</small>",
        clienteProveedor: "Client <br> <small>Supplier</small>",
        proveedorCliente: "Supplier / Client",
        fechaVencimientoNumero: "Expiration Date <br> <small>Invoice Number</small>",
        valorNeto: "Net Value",
        ofertar: "Tender",
        ofertaBanco: "Bank Tender",
        rentabilidad: "Annualized Profitability",
        montoPagar: "Amount to Pay",
        montoRecibir: "Amount to Reciv",
        montoSobrante: "Remains",
        valorTotalLimite: "Assigned",
        opciones: "Options",
        cliente: "Client",
        clientes: "Clients",
        representanteLegal: "Legal Representative",
        moneda: "Coin",
        montoAsignado: "Amount Assigned",
        estado:"State",
        proveedor: "Supplier",
        numeroFactura: "Invoice Number",
        fechaVencimiento: "Expiration Date",
        tipo: "Type",
        numero: "Number",
        monto: "Amount",
        numeroProveedor: "Supplier Number",
        numeroFactura: "Invoice Number",
        tipoFactura: "Invoice Type",
        moneda: "Currency",
        fechaEmision: "Issued Date",
        mensaje: "Message | Messages",
        banco: "Bank",
        financiamiento: "Financing",
        foto: "Image",
        creadoEn: "Created Date",
        correoElectronico: "Email",
        nombre: "Name",
        abreviacion: "Abbreviation",
        discriminador: "Participant",
        confirmar: "Confirm",
        enviar: "Submit",
        eliminar: "Delete",
        comisionServicio: "Commission Service",
        bancoConfirmante: "Confirmant Bank",
        publicar: "Public",
        mejorOferta: "Better Offert",
        ofertar: "Tender",
        gananciaRentabilidad: "Gain / Profitability",
        pago: "Payments",
        apertura: "Opening",
        cierre: "Closing",
        conciliacion: "Conciliation",
        finalizacion: "Ending",
        fechaCreacion: "Created Date",
        fechaAceptacion: "Acceptance Date",
        categoria: "Category",
        referencia: "Reference",

        //Header de Tablas Cuentas Registro
        entidadBancaria: 'Banking entity',
        titular: 'Title',
        numeroCuenta: 'Account number',
        tipoCuenta: 'Account type',
        cuentaPrincipal: 'Main account',
        acciones: 'Options',

        //Header de Tablas Asociados Registro

        numeroDoc: 'N° Document',
        contacto: 'Contact',
        informacion: 'information',
        repLegal: 'Legal representative',
        asociados: 'Relationship',
        estatus: 'State',

        //Header Tabla Consulta
        limiteCredito: "Credit limit",
        doc: "Identification document",
        prefix: "Type",
    },
    mensajesModal: {
        errorSupplierNotAcceptedInvitation: "The provider has not agreed to work with you",
        errorSupplierNotVerified: "The provider has not been verified by the selected bank",
        errorSupplierNotContract: "The provider has not agreed to work with the confirming bank",
        errorDebtorAccountWithEntity: "You do not have an associated account at this bank or have not accepted the terms of service",
        errorRiskLimitNotAvailable: "The Application cannot be made, since it exceeds the available Risk Limit.",
        pagoExito: "Payment has been made successfully.",
        errorLimiteRiesgo: "The Application cannot be made, since it exceeds the available Credit Limit for invoice {0}",
        notFinancingTerm: "Cannot Request Financing Invoices with 0 days Due",
        aplicacionDePagoError: "There has been a problem when making the corresponding Payment Application.",
        supplierHasNotAcceptedDebtorInvitation: "The Supplier has not accepted your invitation to manage Invoices in the System.",
        supplierHasNotAcceptedDebtorInvitationMassive: "The Supplier has not accepted the invitation.",
        pocosRegistros: "Sorry the file must have more than one record",
        creacionDeduccion: "The Invoice Deduction has been successfully created.",
        deleteDeduccion: "The Deduction has been successfully eliminated.",
        crearDeduccionMayorMonto: "There was an inconvenience when creating the Deduction, since the total sum of Deductions exceeds or equals the Net Value.",
        ofertaRealizada:"Your offer was successful",
        estimadoUsuario: "Dear User",
        problemaAceptarOfertaBanco: "There have been problems with Accepting the Offer. Please try again.",
        incovenientesFacturas: "There have been problems with some invoices, for more information check the section Unprocessed invoices.",
        cargaFactura: "Your invoice has been successfully registered.",
        cargaFacturaMasiva: "Your invoice(s) has been registered successfully.",
        postularFactura: "Your invoice has been successfully postulated.",
        financiamientoFactura: "Your Financing Request has been successfully completed.",
        fechaVencimientoTermDays: "The Expiration Date must be more than 4 days from the Current Date.",
        financiamientoFacturaCancelacion: "Your Cancellation Request has been successfully completed.",
        ofertaActualizado: "Your Offer has been successfully updated.",
        aceptarOfertaBanco: "The confirmation of the Offer has been successfully applied.",
        actualizarFactura: "The Invoice Update has been completed successfully.",
        realizarOfertaInversionista: "Your Offer has been sent successfully.",
        actualizarDeduccion: "Your Deduction has been successfully updated.",
        aceptarOfertaCierreMercado: "The Offer has been satisfactorily Accepted.",
        paymentRealized: "Payment has been made satisfactorily",
        rechazarOfertaBanco: "The Rejection of the Offer has been successfully applied.",
        aceptarTodasOfertas: "Offers have been successfully accepted.",
        publicarFactura: "Publication was successful.",
        posponerTodas: "Invoices have been successfully postponed.",
        publicarUnaFactura: "Publication was successful.",
        eliminacionFactura: "The Invoice has been successfully removed.",
        eliminacionFacturaProblema: "There was a problem when trying to delete the Invoice.",
        ofertaRechazada: "Your Offer Rejection has been completed successfully.",
        ofertaAceptada: "Your Acceptance of Offers has been successfully completed.",
        publicacionFactura: "Invoice Publication has been completed successfully.",
        posponerFactura: "The Invoice has been Postponed to Expiration satisfactorily.",
        exitoRespuesta: "Your request has been made successfully | Su solicitud {0} a sido realizada satisfactoriamente",
        problemasRespuesta: "Su solicitud a sido realizada con inconvenientes | Su solicitud {0} a sido realizada con inconvenientes",
        liberarTodasCompraBanco: "Invoices have been successfully released.",
        mercadoCerrado: "Your Offer could not be sent, motivated that the Market is closed.",
        errorSupplierNotBeenVerifier: "Invoice Supplier {0}, has not been verified by the Confirming Bank",
    },
    verifiqueNuevoIntento: "Please check and try again.",
    notConexionDetected: "",
    true: "Active",
    false: "Inactive",
    needName: "You must enter the group name",
    needAbbreviation: "You must enter the abbreviation",
    needProgram: "You must enter the program",
    needDetails: "You must enter details",
    needEvent: "You must enter the event",
    needConcept: "You must enter the concept",
    needCategory: "You must enter the category",
    needAccount: "You must enter the account",
    needCurrency: "You must enter the currency",
    DEBIT: "DEBIT",
    CREDIT: "CREDIT",
    maxNumber: "A maximum of {0} numbers is allowed",
    minNumber: "You must enter minimum {0} numbers",
    addNumberReference: "The reference number is required",
    formatInvalid: "The format you entered is wrong",
    errorConexion: "Problems has been detected to connect to the internet, please check your connection",
    facturaNotUpdate: "This invoice cannot be updated",
    duplicatedEmail: "Duplicate email",
    forgotSend: "A link has been sent to your Email address, to change your Password. Please check",
    requiredPassword: "The {0} is required",
    formatoInvalido: "The Format is invalid ({0})",
    numberDocumentRequired: "Document Number is required",
    passwordRequired: "Password is required",
    typeIdentificationRequired: "The type of document is required",
    errorCaptcha: "Strange behavior has been detected, please enter your details again.",
    proccessInProgress: "Process in Progress",
    errorRespuestaDetalles: "Search failed",
    montoPagarSuperior:"The amount to report is higher than the pending",
    changePassword: "Password successfully changed",
    NUMBER: "Numerical",
    invalidEmailLogin: "The Email format is invalid: goc_08@hotmail.com",
    AMOUNT: "Coin",
    DATE: "Date",
    STRING: "Text",
    PROVEEDOR: "SUPPLIER",
    BANCO: "CONFIRMANT",
    INVERSIONISTA: "FACTOR",
    OPERATIVO: "BACKOFFICE",
    EMPRESA: "DEBTOR",
    invalidRole: "Invalid Role",
    invalidSetting: "Invalid setting",
    invalidAmount: "Invalid amount",
    errorReference: "Invalid reference number",
    errorAccountNumber: "Invalid account number",
    invalidToken: "The supplied Token is invalid",
    expiredToken: "The Token has expired or has already been used",
    sessionExpired: "Your session has expired.",
    passwordChangeLogin: "Password successfully changed, please login to continue",
    publicaFacturaSeguro: "Are you sure to Publish all selected Invoices?",
    posponerFacturaSeguro: "Are you sure to Postpone all selected Invoices?",
    posponerUnaFacturaSeguro: "Are you sure to Postpone the selected Invoice?",
    publicarUnaFacturaSeguro: "Are you sure to Publish the selected Invoice?",
    rechazarBancoSeguro: "Are you sure to Reject all selected Bank Offers?",
    ofertasBancoSeguro: "Are you sure to Accept all selected Bank Offers?",
    confirmacionFacturaIndividual: "Your Confirmation has been completed successfully",
    rechazarConfirmacion: "Invoices have been successfully rejected",
    gananciaRentabilidad: "Gain <br> <small>Profitability</small>",
    montoDescontarAnualizado:"Amount to Discount<br><small>Annualized Cost</small>",
    clienteProveedor: "Debtor<br><small>Supplier</small>",
    fechaVencimientoNumero: "Expiration Date<br><small>Invoice Number</small>",
    errorRespuesta:"Transaction failed",
    problemasRespuesta: "Problems performing transaction",
    exitoRespuesta: "Transaction success",
    tamanoTxtInvalido:"Size of file not huge to 400KB",
    formatoTxtInvalido: "Invalid file format, only plain text files ('file.txt') are allowed",
    errorLeerArchivo: "Error reading file",
    numeroDeduccionDuplicado:"Deduction number already used in this invoice",
    deduccionesSuperanMonto: "Deductions exceed the Net Value of the Invoice.",
    errorAccion: "Error performing the action",
    formatoNumeroInvalido: "Invalid Invoice Number",
    "Key: 'Invoice.Number' Error:Field validation for 'Number' failed on the 'invoiceNumber' tag": "Invalid Invoice Number",
    numeroFacturaVacio: "The selected format is invalid",
    proveedorInvalido: "Invalid Provider",
    proveedorInvalido2: "Supplier not associated with the Client",
    fechaEmisionInvalida: "Invalid issue date",
    fechaEmisionInvalida2: "Issue date must not be less than two years",
    fechaEmisionInvalida3: "Issue date should not be greater than the current date",
    fechaExpiracionInvalida: "Invalid Expiration Date",
    fechaExpiracionInvalida2:  "Expiration date must not exceed 2 years",
    fechaVencimientoInvalida3: "The Expiration Date must be greater than the Current Date",
    fechaVencimientoMenor4: "The Expiration Date must be more than 4 days from the Current Date",
    fechaVencimientoMenorEmision: "Invalid date",
    fechaIguales:  "The issue date must not be greater than or equal to the expiration date",
    archivoVacio: "Empty file",
    montoMayorSuperior: "The Amount must have a maximum of 13 digits in the whole part",
    numeroCamposInvalido7: "Number of incomplete fields (must be 6). Currently there are {0} field(s)",
    numeroCamposInvalidoMas7: "Number of invalid fields (must be 6). There are currently {0} field(s)",
    tipoMoneda: "Invalid currency type",
    tipoFactura: "Invalid invoice type",
    montoInvalido: "Invalid amount",
    valorNetoInvalido:  "Negative net value is not allowed",
    facturaExistente: "Supplier's Invoice is already assigned.",
    facturaInvalida:  "Invalid invoice",
    errorTotal: "An error occurred in a total of",
    facturas: "invoices",
    errorFinanciar: "Error financing invoice",
    errorFinanciarSolicitar:"Error requesting / canceling financing for the invoice",
    subastaCerrada: "Auction closed",
    ofertaInvalida:"Invalid offer",
    ofertaMejor: "Your Offer has not been sent, motivated by a Better Offer",
    ofertaIgual:"Your Offer has not been sent, motivated by a Better Offer",
    cierreNoActivo:"Non-active market close",
    noFinanciarFecha: "Invalid expiration date to request financing",
    invalidUser: "Usuario invalido",
    confirmacionFacturaIndividual: "Your Confirmation has been completed successfully",
    rechazarConfirmacion: "Invoices have been successfully rejected",
    segmentarUsuario: "The Account Executive assignment has been successfully completed.",
    asignarLimiteRiesgo: "The assignment of the Confirming and Financing Lines has been successfully completed.",
    verificarDatos: "The evaluation has been carried out successfully.",
    errorBaseDatos: "An error occurred in the database.",
    CREATED_INVOICE: "Created",
    POSTULATED_INVOICE: "Postulated",
    CONFIRMED_INVOICE: "Confirmed",
    OFFERED_INVOICE: "Offered",
    PUBLISHED_INVOICE: "Published ",
    SOLD_INVOICE: "SOLD ",
    ACCEPTED_OFFER: "Accepted Offer",
    EXPIRED_INVOICE: "Expired ",
    RELEASED_INVOICE: "Released ",
    POSTPONED_INVOICE: "Invoice Awaiting Maturity",
    PAID: "Paid ",
    CONCILIATED: "Conciliated ",
    FINALIZE_INVOICE: "Invoice Completed",
    PROCESSING_INVOICE: "Invoice in Payment Process",
    //Validaciones Registros i18n.t("valid.tipoDoc");
    valid: {
        //Fecha de Nacimiento
        formatoFechaNacimiento: 'Invalid Birth Date Format',

        //Doc Principal
        tipoDoc: 'You must select a Document Type',
        docRequerido: 'The document number is required.',
        formatDoc: 'Invalid Format',
        formatCuenta: 'Invalid Format ',
        docCero: 'Must be greater than 0',
        docDuplicate: 'Document number already registered',
        docAsociadoRep: 'You cannot enter your identity document',

        //Representante y Comercial
        LegalRepRequerido: 'The name of the company is required.',
        replicaLetra: 'Must have at least one letter',
        minimoChar: 'Must be at least 4 characters',
        maxChar: 'Must be 255 characters maximum',
        espacioInit: 'Do not leave initial space',

        //Actividad Comercial
        actComercial: 'You must select a commercial activity',
        cargaConst: "You must associate the file with .PDF format (maximum size 99 kb)",
        ocupation: 'You must choose an occupation',

        //Validar Emails
        emailRequerido: 'Email is required',
        emailFormatoInvalido: 'The email format is invalid: goc_08@hotmail.com',
        emailMaxChar: 'Must have a maximum of 60 characters',

        //Validar Nombres y Apellidos
        segundoNombre: 'You must enter a middle name',
        minimo2Char: 'Must be at least 2 characters',
        charNoPermitidos: 'Only characters from A- Z are allowed, (.), (\')',
        max2CharRepetidos: 'Must not have more than 2 repeated characters in a row',
        nombreRequerido: 'The name is required.',
        nombreAlphNumer: 'The name must have alphabetic values ​​too',
        apellidoRequerido: 'The last name is required',
        charNoPermitidosNombres: 'Only characters from A to Z and characters are allowed (.) y (´)',
        apellidoAlphNumer: 'The last name must have alphabetic values ​​too',

        //Validar Telefono
        telefonoRequerido: 'The phone number is required.',
        telefonoFormatInvalid: 'Invalid Phone Format',
        mayorDeCero: 'Must be greater than 0',

        //Validar Direecion
        direccionPrincipalRequerida: 'The main address is required.',
        direccionAlphNumer: 'The address must have alphabetic characters too',
        direccionMaxChar: 'Must be at least 15 characters',
        seleccionEstado: 'You must select a state',
        seleccionCiudad: 'You must select a city',

        //Validar Codigos Bancos
        codigoBank: "The bank code is mandatory",
        codigoBankExist: "Bank code already exist",
        maxCodigoBank: "You must enter a maximum of 16 digits",
        minimoCharBanco: 'You must enter a minimum of 3 digits',
        logoBank: "please only images .jpeg /.png",

        //Validar Cuenta Bancaria
        cuentaBanco: 'You must select a bank',
        cuentaMoneda: 'You must select a currency type',
        cuentaTipo: 'You must select an account type',
        titularRequerido: 'The full name of the holder is required',
        titular2NombresMin: 'Must have a first and last name with at least two letters each',
        numeroCuentaRequerido: 'The account number is required',
        minNumCuenta: 'Must have at least 20 digits',
        maxNumCuenta: 'Must have a maximum of 20 digits',
        numCuentaExistente: 'The account number is already associated with the company',
        docNumberDuplicado: 'Document number already registered',
        cuentaBancoUnica: 'Only one account is allowed per bank',
        codigoBancarioIncorrecto: 'The entered code does not belong to the bank.',

        //Valiar Usuario Existente
        usuarioExistenteProveedor: 'The provider is already registered',
        usuarioExistenteEmpresa: 'The company is already registered',
        usuarioExistenteFactor: 'The investor is already registered',
        usuarioExistenteBanco: 'The bank is already registered',
        numeroDocDuplicado: 'Numero de documento existente',

        //Verificar Datos
        comentarioRechazoRequerido: 'A comment is required to continue',
        comentarioRechazoLetras: 'must include some letter',
        comentarioRechazoMin: 'The comment must consist of a minimum of 15 letters',
        comentarioRechazoMax: 'The comment must consist of a maxinimum of 100 letters',

        //Verificar Limite de Cuenta
        limiteMin: 'required field, the minimum amount allowed is 1,00',
    },

    //Tooltip Mensajes
    tooltip: {

        queridoUsuario: 'Dear user',
        registroEmpresa: "Your information has been successfully registered and will be verified.",
        registroProveedor: "Your information has been satisfactorily registered and will be verified.You must accept the Framework Contract to begin Operations in the 'TuFactoring' System",
        registroBanco: "The registration of the Allied Bank has been carried out satisfactorily.",
        registroEmpresaBanco: "The Client has registered successfully.",
        actualizarPerfil: "Your information has been updated successfully.",
        errorUsuarioRegistrado: "The user is already registered.",
        liberarFacturas: "The invoice(s) has been successfully released.",
        conciliarPago: "The Reconciliation has been carried out satisfactorily.",
        rechazarPago: "The Settlement has been Rejected and the Investor has been Blocked.",
        contratoMarcoPendiente: "You have Pending Allied Bank Contract(s) to accept.Please check your Profile.",
        contratoTerminos: "The Terms and Conditions have been satisfactorily accepted.",
        contratoMarco: "The Framework Contract has been satisfactorily accepted.",
        limiteCreditoSinCuentaBancaria: "The Credit Limit cannot be assigned, due to the fact that the Client does not have an associated Account in the Bank.",
        noAuthorizado: "You are not authorised to perform this action",
        internalError: "internal system error",
        archivoConciliar: "The reconciliation file has been successfully processed.",
        formatoDocumento: "The check digit of the document number does not match.",
        formatoCuentaBancaria: "The Unique Banking Code does not match any registered bank",
    }

}
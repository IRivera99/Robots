Sky.net ha solicitado que continuemos el development del programa que realizamos originalmente, y nos han llegado nuevos requerimientos.

Sky.Net tiene un terreno de 100 kilómetros cuadrados para iterar diferentes pruebas de movilidad y rendimiento en sus operadores. Nos han pedido que creemos una simulación dinámica de este terreno donde podamos randomizar diferentes entornos, donde cada kilómetro cuadrado represente un tipo de localización.

Estas localizaciones se clasifican por tipo.

Tipos de localización:
	- Terreno baldío, planicie, bosque, sector urbano: terrenos comunes sin efectos
	- Vertedero: Sector lleno de basura, al pasar hay una chance de 5% de dañar componentes del Operador. Debemos simular esto con un randomizador.
	- Lago: Un sector inundado, las unidades K9 y M8 no pueden pasar.
	- Vertedero electrónico: Un sector lleno de basura electrónica, no tiene chance de causar daño físico al operador pero las ondas electromagnéticas de los dispositivos dañan las baterías y reducen su capacidad máxima en un 20% permanentemente.
	- Cuartel: un punto de control donde los operadores pueden recargar batería o ser reparados. Pueden existir varios, pero nunca más de 3.
	- Sitio de reciclaje: Un sector dedicado a transformar basura en recursos útiles. Estos sitios poseen puntos de recarga para los operadores pero no de mantenimiento. Existen un máximo de 5 en todo el terreno.
	
Nos han solicitado la siguiente funcionalidad:
1) Simular el terreno de 100 kilómetros cuadrados con varios tipos de localizaciones generados aleatoriamente.
2) Actualizar las rutinas de movimiento - no es necesario codear un movimiento diagonal.
	A) Generar rutinas para ordenar un operador moverse a una coordenada o localización en especial
	B) Opcional: Programar una rutina que genere una ruta óptima (es decir, sin peligro) y otra que genere una ruta directa.
3) Persistencia de datos. Al iniciar el programa, nos debe preguntar si queremos cargar una simulación previa o generar una nueva simulación.
	A) Nos han pedido que guardemos todos los datos relevantes: El terreno de 100*100 y el estado de todos los operadores. Somos libres de decidir cómo y cuándo hacerlo.
	B) Somos libres de utilizar el sistema que creamos óptimo para guardar datos, pero debe ser dinámico para que pueda utilizarse en otras computadoras.
4) Actualizar rutinas generales para adaptarse a nuestro nuevo diseño.
5) Opcional: Sistema de tiempo. No nos han solicitado un sistema para calcular el tiempo de operaciones o asignar órdenes en ‘tiempo real’. Un sistema tal sería laborioso, pero muy recompensante.

5) Nueva funcionalidad:
	A) Orden general: Todos los operadores que no estén ocupados actualmente deben dirigirse al vertedero más cercano y recoger su cantidad máxima de carga para traer al sitio de reciclaje más cercano.
	B) Orden general: Todos los operadores que estén dañados deben volver a un cuartel para mantenimiento.
	C) Cambiar Batería: Reemplaza una batería dañada.
	D) Simular daño: Un operador puede sufrir estos diferentes daños:
		>MOTOR COMPROMETIDO: Reduce su velocidad promedio a la mitad.
		>SERVO ATASCADO: No puede realizar operaciones de carga y descarga física
		>BATERIA PERFORADA: Pierde batería un 500% más rápido en cada operación
		>PUERTO BATERIA DESCONECTADO: No puede realizar operaciones de carga, recarga o transferencia de batería
		>PINTURA RAYADA: No tiene efecto
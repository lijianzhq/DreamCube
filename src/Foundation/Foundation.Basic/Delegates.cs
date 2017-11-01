using System;

#if NET20

public delegate void Action();
public delegate void Action<T1, T2>(T1 t1, T2 t2);
public delegate void Action<T1, T2, T3>(T1 t1, T2 t2, T3 t3);
public delegate void Action<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);

public delegate Tout Func<Tout>();
public delegate Tout Func<Tin, Tout>(Tin tin);
public delegate Tout Func<Tin1, Tin2, Tout>(Tin1 tin1, Tin2 tin2);
public delegate Tout Func<Tin1, Tin2, Tin3, Tout>(Tin1 arg1, Tin2 arg2, Tin3 arg3);
public delegate Tout Func<Tin1, Tin2, Tin3, Tin4, Tout>(Tin1 arg1, Tin2 arg2, Tin3 arg3, Tin4 arg4);

#endif

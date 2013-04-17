using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastruktur
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

        public sealed class TypeSwitcher<T>
        {
            private readonly List<Action<T>> _before = new List<Action<T>>();
            private readonly List<Func<T, bool>> _handle = new List<Func<T, bool>>();
            private readonly List<Action<T>> _after = new List<Action<T>>();

            private TypeSwitcher()
            {
            }

            public static TypeSwitcher<T> Create()
            {
                return new TypeSwitcher<T>();
            }

            public TypeSwitcher<T> Before(Action<T> action)
            {
                _before.Add(action);
                return this;
            }

            public TypeSwitcher<T> Before(Action action)
            {
                _before.Add(t => action());
                return this;
            }

            public TypeSwitcher<T> After(Action action)
            {
                _after.Add(t => action());
                return this;
            }

            public TypeSwitcher<T> After(Action<T> action)
            {
                _after.Add(action);
                return this;
            }

            public TypeSwitcher<T> On<TCase>(Action<TCase> action, bool skipFurther = true) where TCase : class, T
            {
                _handle.Add(t =>
                {
                    var c = t as TCase;
                    if (c != null)
                    {
                        action(c);
                        return skipFurther;
                    }
                    return false;
                });

                return this;
            }

            public void Run(IEnumerable<T> input)
            {
                foreach (var t in input)
                {
                    _before.ForEach(action => action(t));
                    foreach (var h in _handle) if (h(t)) break;
                    _after.ForEach(action => action(t));
                }
            }
        }


        public sealed class TypeSwitcher<TSource, TIntermediate>
        {
            private readonly Func<TSource, TIntermediate> _unwrap;
            private readonly List<Action<TSource, TIntermediate>> _before = new List<Action<TSource, TIntermediate>>();
            private readonly List<Func<TSource, TIntermediate, bool>> _handle = new List<Func<TSource, TIntermediate, bool>>();
            private readonly List<Action<TSource, TIntermediate>> _after = new List<Action<TSource, TIntermediate>>();

            private TypeSwitcher(Func<TSource, TIntermediate> unwrap)
            {
                _unwrap = unwrap;
            }

            public static TypeSwitcher<TSource, TIntermediate> Create(Func<TSource, TIntermediate> unwrap)
            {
                return new TypeSwitcher<TSource, TIntermediate>(unwrap);
            }

            public TypeSwitcher<TSource, TIntermediate> Before(Action<TSource, TIntermediate> action)
            {
                _before.Add(action);
                return this;
            }

            public TypeSwitcher<TSource, TIntermediate> Before(Action action)
            {
                _before.Add((src, t) => action());
                return this;
            }

            public TypeSwitcher<TSource, TIntermediate> After(Action action)
            {
                _after.Add((src, t) => action());
                return this;
            }

            public TypeSwitcher<TSource, TIntermediate> After(Action<TSource, TIntermediate> action)
            {
                _after.Add(action);
                return this;
            }

            public TypeSwitcher<TSource, TIntermediate> On<TCase>(Action<TCase, TSource> action, bool skipFurther = true) where TCase : class, TIntermediate
            {
                _handle.Add((src, t) =>
                {
                    var c = t as TCase;
                    if (c != null)
                    {
                        action(c, src);
                        return skipFurther;
                    }
                    return false;
                });

                return this;
            }

            public void Run(IEnumerable<TSource> input)
            {
                foreach (var src in input)
                {
                    var t = _unwrap(src);
                    _before.ForEach(action => action(src, t));
                    foreach (var h in _handle) if (h(src, t)) break;
                    _after.ForEach(action => action(src, t));
                }
            }
        }

    }

﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SimpleNodeEditor
{
    [System.Serializable]
    public abstract class BaseNode 
        : MonoBehaviour
    {
        public bool Visible = true;

        [SerializeField]
        protected Rect m_rect;
        public Rect Rect { get { return m_rect; } }
        public int Id = 0;

        public bool ShowCloseButton = true;

        private string m_name = "Node";
        public string Name
        {
            get
            {
                return m_name;
            }
            set
            {
                gameObject.name = value;
                m_name = value;
            }
        }

        [SerializeField]
        private Vector2 m_position = new Vector2(10,10);
        public Vector2 Position
        {
            set
            {
                m_position = value;
                m_rect = new Rect(m_position.x, m_position.y, m_size.x, m_size.y);
            }
            get
            {
                return m_position;
            }
        }
       
        [SerializeField]
        private Vector2 m_size = new Vector2(100,100);
        public Vector2 Size
        {
            set
            {
                m_size = value;
                m_rect = new Rect(Position.x, Position.y, m_size.x, m_size.y);

                // Reposition outlets if size changes
                for(int i = 0 ; i < m_lets.Count; i++ )
                {
                    Outlet outlet = m_lets[i] as Outlet;

                    if(outlet!=null)
                    {
                        outlet.Offset.x = Size.x - 5;
                    }
                }
            }
            get
            {
                return m_size;
            }
        }

        protected bool m_valid = true;
        public bool Valid { get { return m_valid; } }

        [SerializeField]
        protected List<Let> m_lets = new List<Let>();
        public List<Let> Lets { get { return m_lets; } }

        [SerializeField]
        protected Rect m_closeBoxPos = new Rect(10, -20, 10, 20);

        protected T MakeLet <T> (string name = "Let", int yOffset = 0) where T : Let
        {
            T let = gameObject.AddComponent<T>();
            let.Construct(this);
            m_lets.Add(let);

            let.Name = name;
            let.yOffset = yOffset;

            return let;
        }

        protected void DestroyLet(Let let)
        {
            m_lets.Remove(let);
            DestroyImmediate(let);
        }

        void Start()
        {
            Inited();
        }

        public abstract void Construct();
        protected abstract void Inited();

#if UNITY_EDITOR
        public void Draw()
        {
            if (!Visible)
                return;

            m_rect = GUI.Window(Id, m_rect, WindowCallback, gameObject.name);

            Vector2 newPos = new Vector2(m_rect.x, m_rect.y);

            if( newPos != Position )
            {
                for(int i = 0; i < m_lets.Count; i++ )
                {
                    switch(m_lets[i].Type)
                    {
                        case LetTypes.INLET:
                            Inlet inlet = m_lets[i] as Inlet;
                            for (int j = 0; j < inlet.Connections.Count; j++ )
                            {
                                inlet.Connections[j].Outlet.MakeConnections();
                            }
                            break;
                        case LetTypes.OUTLET:
                            Outlet outlet = m_lets[i] as Outlet;
                            outlet.MakeConnections();
                            break;
                    }
                }
            }

            Position = newPos;
            m_size = new Vector2(m_rect.width, m_rect.height);

            // Draw Let(s)
            for (int i = 0; i < m_lets.Count; i++)
            {
                m_lets[i].DrawLet(m_rect);
            }

            // draw close box
            if( ShowCloseButton )
                GUI.Box(m_closeBoxPos, "X");
        }

        public virtual bool MouseOver(Vector2 mousePos)
        {
            if (!Visible)
                return false;

            bool handled = false;
            for (int i = 0; i < m_lets.Count; i++)
            {
                if (m_lets[i].MouseOver(mousePos))
                {
                    handled = true;
                    break;
                }
            }

            return handled;
        }

        public virtual bool MouseDrag(Vector2 mousePos)
        {
            if (!Visible)
                return false;
            
            bool handled = false;
            for (int i = 0; i < m_lets.Count; i++)
            {
                if (m_lets[i].MouseDrag(mousePos))
                {
                    handled = true;
                    break;
                }
            }

            return handled;
        }

        public virtual bool MouseDown(Vector2 mousePos, int button)
        {
            if (!Visible)
                return false;


            bool handled = false;
            for (int i = 0; i < m_lets.Count; i++)
            {
                if (m_lets[i].MouseDown(mousePos, button))
                {
                    handled = true;
                    break;
                }
            }

            // check if mouse is on close box if mouseevent is not handled by Input
            if (!handled && m_closeBoxPos.Contains(mousePos) && ShowCloseButton)
            {
                m_valid = false;
            }

            return handled;
        }

        public virtual bool MouseUp(Vector2 mousePos)
        {
            if (!Visible)
                return false;

            bool handled = false;
            for (int i = 0; i < m_lets.Count; i++)
            {
                if (m_lets[i].MouseUp(mousePos))
                {
                    handled = true;
                    break;
                }
            }

            return handled;
        }

        public virtual void WindowCallback(int id)
        {
            if (!Visible)
                return;

            GUI.DragWindow();

            for (int i = 0; i < m_lets.Count; i++)
            {
                m_lets[i].DrawLabel();
            }

            m_closeBoxPos = new Rect(Position.x + 5, Position.y - 25, 20, 20);
        }
#endif

        public void BreakAllLets()
        {
            foreach (Let let in m_lets)
            {
                let.BreakAllConnections();
            }
        }

        void OnDestroy()
        {
            BreakAllLets();
        }
    }
}
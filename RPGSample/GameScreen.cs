using System;
using Microsoft.Xna.Framework;
namespace RPGSample
{
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden
    }
    public abstract class GameScreen
    {
        bool isPopup = false;
        public bool IsPopup
        {
            /// Normally when one screen is brought up over the top of another,
            /// the first screen will transition off to make room for the new
            /// one. This property indicates whether the screen is only a small
            /// popup, in which case screens underneath it do not need to bother
            /// transitioning off.
            
            get { return isPopup; }
            protected set { isPopup = value; }
        }
        TimeSpan transitionOnTime = TimeSpan.Zero;
        public TimeSpan TransitionOnTime
        {
            /// Indicates how long the screen takes to
            /// transition on when it is activated.
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }
        TimeSpan transitionOffTime = TimeSpan.Zero;
        public TimeSpan TransitionOffTime
        {
            /// Indicates how long the screen takes to
            /// transition off when it is deactivated.
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }
        float transitionPosition = 1;
        public float TransitionPosition
        {
            /// Gets the current position of the screen transition, ranging
            /// from zero (fully active, no transition) to one (transitioned
            /// fully off to nothing).
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }
        public byte TransitionAlpha
        {
            /// Gets the current alpha of the screen transition, ranging
            /// from 255 (fully active, no transition) to 0 (transitioned
            /// fully off to nothing).
            get { return (byte)(255 - TransitionPosition * 255); }
        }
        ScreenState screenState = ScreenState.TransitionOn;
        public ScreenState ScreenState
        {
            /// Gets the current screen transition state.
            get { return screenState; }
            protected set { screenState = value; }
        }
        bool isExiting = false;
        public event EventHandler Exiting;
        public bool IsExiting
        {
            /// There are two possible reasons why a screen might be transitioning
            /// off. It could be temporarily going away to make room for another
            /// screen that is on top of it, or it could be going away for good.
            /// This property indicates whether the screen is exiting for real:
            /// if set, the screen will automatically remove itself as soon as the
            /// transition finishes.
            get { return isExiting; }
            protected internal set
            {
                bool fireEvent = !isExiting && value;
                isExiting = value;
                if (fireEvent && (Exiting != null))
                {
                    Exiting(this, EventArgs.Empty);
                }
            }
        }
        bool otherScreenHasFocus;
        public bool IsActive
        {
            /// Checks whether this screen is active and can respond to user input.
            get
            {
                return !otherScreenHasFocus &&
                       (screenState == ScreenState.TransitionOn ||
                        screenState == ScreenState.Active);
            }
        }

        ScreenManager screenManager;
        /// <summary>
        /// Gets the manager that this screen belongs to.
        /// </summary>
        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }
        /// <summary>
        /// Load graphics content for the screen.
        /// </summary>
        public virtual void LoadContent() { }


        /// <summary>
        /// Unload content for the screen.
        /// </summary>
        public virtual void UnloadContent() { }

        /// <summary>
        /// Allows the screen to handle user input. Unlike Update, this method
        /// is only called when the screen is active, and not when some other
        /// screen has taken the focus.
        /// </summary>
        public virtual void HandleInput() { }


        /// <summary>
        /// This is called when the screen should draw itself.
        /// </summary>
        public virtual void Draw(GameTime gameTime) { }

        /// <summary>
        /// Allows the screen to run logic, such as updating the transition position.
        /// Unlike HandleInput, this method is called regardless of whether the screen
        /// is active, hidden, or in the middle of a transition.
        /// </summary>
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (IsExiting)
            {
                // If the screen is going away to die, it should transition off.
                screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // When the transition finishes, remove the screen.
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
                // If the screen is covered by another, it should transition off.
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    // Still busy transitioning.
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    // Transition finished!
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
                // Otherwise the screen should transition on and become active.
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    // Still busy transitioning.
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    // Transition finished!
                    screenState = ScreenState.Active;
                }
            }
        }

        /// <summary>
        /// Helper for updating the screen transition position.
        /// </summary>
        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            // How much should we move by?
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

            // Update the transition position.
            transitionPosition += transitionDelta * direction;

            // Did we reach the end of the transition?
            if ((transitionPosition <= 0) || (transitionPosition >= 1))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }

            // Otherwise we are still busy transitioning.
            return true;
        }

        /// <summary>
        /// Tells the screen to go away. Unlike ScreenManager.RemoveScreen, which
        /// instantly kills the screen, this method respects the transition timings
        /// and will give the screen a chance to gradually transition off.
        /// </summary>
        public void ExitScreen()
        {
            // flag that it should transition off and then exit.
            IsExiting = true;
            // If the screen has a zero transition time, remove it immediately.
            if (TransitionOffTime == TimeSpan.Zero)
            {
                ScreenManager.RemoveScreen(this);
            }
        }
    }
}

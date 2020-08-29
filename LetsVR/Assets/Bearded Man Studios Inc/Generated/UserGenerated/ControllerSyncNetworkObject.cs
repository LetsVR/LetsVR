using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15,0.15,0,0,0,0.15,0.15]")]
	public partial class ControllerSyncNetworkObject : NetworkObject
	{
		public const int IDENTITY = 2;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector3 _position;
		public event FieldEvent<Vector3> positionChanged;
		public InterpolateVector3 positionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 position
		{
			get { return _position; }
			set
			{
				// Don't do anything if the value is the same
				if (_position == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_position = value;
				hasDirtyFields = true;
			}
		}

		public void SetpositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_position(ulong timestep)
		{
			if (positionChanged != null) positionChanged(_position, timestep);
			if (fieldAltered != null) fieldAltered("position", _position, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _rotation;
		public event FieldEvent<Quaternion> rotationChanged;
		public InterpolateQuaternion rotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion rotation
		{
			get { return _rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetrotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_rotation(ulong timestep)
		{
			if (rotationChanged != null) rotationChanged(_rotation, timestep);
			if (fieldAltered != null) fieldAltered("rotation", _rotation, timestep);
		}
		[ForgeGeneratedField]
		private bool _isLeftController;
		public event FieldEvent<bool> isLeftControllerChanged;
		public Interpolated<bool> isLeftControllerInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool isLeftController
		{
			get { return _isLeftController; }
			set
			{
				// Don't do anything if the value is the same
				if (_isLeftController == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_isLeftController = value;
				hasDirtyFields = true;
			}
		}

		public void SetisLeftControllerDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_isLeftController(ulong timestep)
		{
			if (isLeftControllerChanged != null) isLeftControllerChanged(_isLeftController, timestep);
			if (fieldAltered != null) fieldAltered("isLeftController", _isLeftController, timestep);
		}
		[ForgeGeneratedField]
		private ulong _playerIdentifier;
		public event FieldEvent<ulong> playerIdentifierChanged;
		public Interpolated<ulong> playerIdentifierInterpolation = new Interpolated<ulong>() { LerpT = 0f, Enabled = false };
		public ulong playerIdentifier
		{
			get { return _playerIdentifier; }
			set
			{
				// Don't do anything if the value is the same
				if (_playerIdentifier == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_playerIdentifier = value;
				hasDirtyFields = true;
			}
		}

		public void SetplayerIdentifierDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_playerIdentifier(ulong timestep)
		{
			if (playerIdentifierChanged != null) playerIdentifierChanged(_playerIdentifier, timestep);
			if (fieldAltered != null) fieldAltered("playerIdentifier", _playerIdentifier, timestep);
		}
		[ForgeGeneratedField]
		private bool _isHandsMode;
		public event FieldEvent<bool> isHandsModeChanged;
		public Interpolated<bool> isHandsModeInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool isHandsMode
		{
			get { return _isHandsMode; }
			set
			{
				// Don't do anything if the value is the same
				if (_isHandsMode == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x10;
				_isHandsMode = value;
				hasDirtyFields = true;
			}
		}

		public void SetisHandsModeDirty()
		{
			_dirtyFields[0] |= 0x10;
			hasDirtyFields = true;
		}

		private void RunChange_isHandsMode(ulong timestep)
		{
			if (isHandsModeChanged != null) isHandsModeChanged(_isHandsMode, timestep);
			if (fieldAltered != null) fieldAltered("isHandsMode", _isHandsMode, timestep);
		}
		[ForgeGeneratedField]
		private Vector3 _pointerPosition;
		public event FieldEvent<Vector3> pointerPositionChanged;
		public InterpolateVector3 pointerPositionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 pointerPosition
		{
			get { return _pointerPosition; }
			set
			{
				// Don't do anything if the value is the same
				if (_pointerPosition == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x20;
				_pointerPosition = value;
				hasDirtyFields = true;
			}
		}

		public void SetpointerPositionDirty()
		{
			_dirtyFields[0] |= 0x20;
			hasDirtyFields = true;
		}

		private void RunChange_pointerPosition(ulong timestep)
		{
			if (pointerPositionChanged != null) pointerPositionChanged(_pointerPosition, timestep);
			if (fieldAltered != null) fieldAltered("pointerPosition", _pointerPosition, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _pointerRotation;
		public event FieldEvent<Quaternion> pointerRotationChanged;
		public InterpolateQuaternion pointerRotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion pointerRotation
		{
			get { return _pointerRotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_pointerRotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x40;
				_pointerRotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetpointerRotationDirty()
		{
			_dirtyFields[0] |= 0x40;
			hasDirtyFields = true;
		}

		private void RunChange_pointerRotation(ulong timestep)
		{
			if (pointerRotationChanged != null) pointerRotationChanged(_pointerRotation, timestep);
			if (fieldAltered != null) fieldAltered("pointerRotation", _pointerRotation, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			positionInterpolation.current = positionInterpolation.target;
			rotationInterpolation.current = rotationInterpolation.target;
			isLeftControllerInterpolation.current = isLeftControllerInterpolation.target;
			playerIdentifierInterpolation.current = playerIdentifierInterpolation.target;
			isHandsModeInterpolation.current = isHandsModeInterpolation.target;
			pointerPositionInterpolation.current = pointerPositionInterpolation.target;
			pointerRotationInterpolation.current = pointerRotationInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _rotation);
			UnityObjectMapper.Instance.MapBytes(data, _isLeftController);
			UnityObjectMapper.Instance.MapBytes(data, _playerIdentifier);
			UnityObjectMapper.Instance.MapBytes(data, _isHandsMode);
			UnityObjectMapper.Instance.MapBytes(data, _pointerPosition);
			UnityObjectMapper.Instance.MapBytes(data, _pointerRotation);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_position = UnityObjectMapper.Instance.Map<Vector3>(payload);
			positionInterpolation.current = _position;
			positionInterpolation.target = _position;
			RunChange_position(timestep);
			_rotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			rotationInterpolation.current = _rotation;
			rotationInterpolation.target = _rotation;
			RunChange_rotation(timestep);
			_isLeftController = UnityObjectMapper.Instance.Map<bool>(payload);
			isLeftControllerInterpolation.current = _isLeftController;
			isLeftControllerInterpolation.target = _isLeftController;
			RunChange_isLeftController(timestep);
			_playerIdentifier = UnityObjectMapper.Instance.Map<ulong>(payload);
			playerIdentifierInterpolation.current = _playerIdentifier;
			playerIdentifierInterpolation.target = _playerIdentifier;
			RunChange_playerIdentifier(timestep);
			_isHandsMode = UnityObjectMapper.Instance.Map<bool>(payload);
			isHandsModeInterpolation.current = _isHandsMode;
			isHandsModeInterpolation.target = _isHandsMode;
			RunChange_isHandsMode(timestep);
			_pointerPosition = UnityObjectMapper.Instance.Map<Vector3>(payload);
			pointerPositionInterpolation.current = _pointerPosition;
			pointerPositionInterpolation.target = _pointerPosition;
			RunChange_pointerPosition(timestep);
			_pointerRotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			pointerRotationInterpolation.current = _pointerRotation;
			pointerRotationInterpolation.target = _pointerRotation;
			RunChange_pointerRotation(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _isLeftController);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _playerIdentifier);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _isHandsMode);
			if ((0x20 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _pointerPosition);
			if ((0x40 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _pointerRotation);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (positionInterpolation.Enabled)
				{
					positionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					positionInterpolation.Timestep = timestep;
				}
				else
				{
					_position = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_position(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (rotationInterpolation.Enabled)
				{
					rotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					rotationInterpolation.Timestep = timestep;
				}
				else
				{
					_rotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_rotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (isLeftControllerInterpolation.Enabled)
				{
					isLeftControllerInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					isLeftControllerInterpolation.Timestep = timestep;
				}
				else
				{
					_isLeftController = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_isLeftController(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (playerIdentifierInterpolation.Enabled)
				{
					playerIdentifierInterpolation.target = UnityObjectMapper.Instance.Map<ulong>(data);
					playerIdentifierInterpolation.Timestep = timestep;
				}
				else
				{
					_playerIdentifier = UnityObjectMapper.Instance.Map<ulong>(data);
					RunChange_playerIdentifier(timestep);
				}
			}
			if ((0x10 & readDirtyFlags[0]) != 0)
			{
				if (isHandsModeInterpolation.Enabled)
				{
					isHandsModeInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					isHandsModeInterpolation.Timestep = timestep;
				}
				else
				{
					_isHandsMode = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_isHandsMode(timestep);
				}
			}
			if ((0x20 & readDirtyFlags[0]) != 0)
			{
				if (pointerPositionInterpolation.Enabled)
				{
					pointerPositionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					pointerPositionInterpolation.Timestep = timestep;
				}
				else
				{
					_pointerPosition = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_pointerPosition(timestep);
				}
			}
			if ((0x40 & readDirtyFlags[0]) != 0)
			{
				if (pointerRotationInterpolation.Enabled)
				{
					pointerRotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					pointerRotationInterpolation.Timestep = timestep;
				}
				else
				{
					_pointerRotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_pointerRotation(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (positionInterpolation.Enabled && !positionInterpolation.current.UnityNear(positionInterpolation.target, 0.0015f))
			{
				_position = (Vector3)positionInterpolation.Interpolate();
				//RunChange_position(positionInterpolation.Timestep);
			}
			if (rotationInterpolation.Enabled && !rotationInterpolation.current.UnityNear(rotationInterpolation.target, 0.0015f))
			{
				_rotation = (Quaternion)rotationInterpolation.Interpolate();
				//RunChange_rotation(rotationInterpolation.Timestep);
			}
			if (isLeftControllerInterpolation.Enabled && !isLeftControllerInterpolation.current.UnityNear(isLeftControllerInterpolation.target, 0.0015f))
			{
				_isLeftController = (bool)isLeftControllerInterpolation.Interpolate();
				//RunChange_isLeftController(isLeftControllerInterpolation.Timestep);
			}
			if (playerIdentifierInterpolation.Enabled && !playerIdentifierInterpolation.current.UnityNear(playerIdentifierInterpolation.target, 0.0015f))
			{
				_playerIdentifier = (ulong)playerIdentifierInterpolation.Interpolate();
				//RunChange_playerIdentifier(playerIdentifierInterpolation.Timestep);
			}
			if (isHandsModeInterpolation.Enabled && !isHandsModeInterpolation.current.UnityNear(isHandsModeInterpolation.target, 0.0015f))
			{
				_isHandsMode = (bool)isHandsModeInterpolation.Interpolate();
				//RunChange_isHandsMode(isHandsModeInterpolation.Timestep);
			}
			if (pointerPositionInterpolation.Enabled && !pointerPositionInterpolation.current.UnityNear(pointerPositionInterpolation.target, 0.0015f))
			{
				_pointerPosition = (Vector3)pointerPositionInterpolation.Interpolate();
				//RunChange_pointerPosition(pointerPositionInterpolation.Timestep);
			}
			if (pointerRotationInterpolation.Enabled && !pointerRotationInterpolation.current.UnityNear(pointerRotationInterpolation.target, 0.0015f))
			{
				_pointerRotation = (Quaternion)pointerRotationInterpolation.Interpolate();
				//RunChange_pointerRotation(pointerRotationInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public ControllerSyncNetworkObject() : base() { Initialize(); }
		public ControllerSyncNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public ControllerSyncNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}

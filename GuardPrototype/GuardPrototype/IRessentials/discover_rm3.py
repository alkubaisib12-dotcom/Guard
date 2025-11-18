
import broadlink
import json

devices = broadlink.discover(timeout=5)
user_devices = []

for dev in devices:
    try:
        dev.auth()
        user_devices.append({
            "name": dev.host[0],
            "mac": dev.mac.hex(),
            "type": dev.devtype
        })
    except Exception as e:
        print(f"Failed to authenticate {dev.host}: {e}")

print(json.dumps(user_devices))
